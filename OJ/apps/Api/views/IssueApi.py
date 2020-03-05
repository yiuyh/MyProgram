import re
import time

from django.core.paginator import Paginator
from rest_framework.views import APIView
from rest_framework import serializers

from Issue.models import Problem
from django.http import JsonResponse
from Issue.serializers import *

#获取用户的问题的通过状态
from UserProfile.util import check_auth


def is_accepted(user, probs):
    ac_List = []
    for prob in probs:
        try:
            if ProblemSubmit.objects.filter(user__id=user.id, problem__no=prob["no"], result='1').exists():
                ac_List.append('yes')
            elif ProblemSubmit.objects.filter(user__id=user.id, problem__no=prob["no"]).exists():
                ac_List.append('no')
            else:
                ac_List.append('')
        except:
            ac_List.append('')
    return ac_List


#获取问题列表
class GetProblemPage(APIView):
    def get(self, request):  #按（id|名字|类型|来源）查询第page页的问题列表

        problem_Id_Name = request.GET.get('problem_Id_Name', '')
        problem_Source = request.GET.get('problem_Source', 'All')
        algorithm_Type = request.GET.get('algorithm_Type', 'All')
        query_Criteria = {}  # 创建一个多条件查询字典
        query_Criteria['probAuthority'] = '公开'

        if problem_Id_Name != '':
            try:  # 尝试将参数作为数值处理，并当作id搜索，如果不行，就转化为name进行搜索
                query_Criteria['no'] = int(problem_Id_Name)
            except:
                query_Criteria['title__iregex'] = '.*'.join(problem_Id_Name)
        if problem_Source != '' and problem_Source != 'All':
            query_Criteria['probSource'] = problem_Source
        if algorithm_Type != '' and algorithm_Type != 'All':
            query_Criteria['classification'] = algorithm_Type
        problems = Problem.objects.filter(**query_Criteria)
        # 分页处理

        paginator_OfProblemsAll = Paginator(problems, 20)
        page_Num = request.GET.get('page', 1)
        paginator_OfProblemsAll = paginator_OfProblemsAll.page(page_Num)
        paginator_OfProblemsAll = paginator_OfProblemsAll.object_list
        paginator_OfProblemsAll = ProblemListSerializer(paginator_OfProblemsAll, many = True).data

        is_Login, user = check_auth(request._request)
        ac_Flag = is_accepted(user, paginator_OfProblemsAll)

        # 将这些数据返回到前端，这样便于进行多条件过滤
        data = {'status': 200,'msg': '已获取所有问题','data': {}}
        data["data"].update({'problems': paginator_OfProblemsAll, 'ac_flag': ac_Flag,
                       'now_page': page_Num, 'problem_id_name': problem_Id_Name,
                       'problem_source': problem_Source, 'algorithm_type': algorithm_Type})

        return JsonResponse(data)


#获取单个问题或修改单个问题
class GetProblem(APIView):

    def get(self, request, prob_id):
        data = {'status': 200, 'msg': '成功获取题目','data':{}}
        try:
            #获取问题
            prob_no = str(prob_id)

            problem = Problem.objects.get(no=prob_no, probAuthority='公开')

            data["data"].update({"problem":problem.get_problem()})
            print(prob_no)

            code = ''
            language = 'C'

            # 如果用户登录 则获取用户最新一次提交代码
            is_Login, user = check_auth(request._request)
            try:
                problem_Submit = ProblemSubmit.objects.filter(problem__no=prob_no, user__id=user.id).last()
                language = problem_Submit.language
                code = problem_Submit.content
            except:
                pass
            data["data"].update({"language":language, "code":code})
        except:
            data['msg'] = '题目不存在'
            data['status'] = 400
        return JsonResponse(data=data)


    def post(self, request, prob_id):
        data = {'status': 200, 'msg': '修改成功', 'data': {}}
        try:
            problem = Problem.objects.get(no = prob_id)
            problem.updata_problem(request.data)
        except:
            data['msg'] = '问题不存在'
            data['status'] = 400

        return JsonResponse(data=data)


#创建问题
class CreateProblem(APIView):
    def post(self):
        data = {'status': 200, 'msg': '已获取所有问题', 'data': {}}

        #身份验证








def change_status(status_List):  # 提交状态
    for item in status_List:
        if item.result == '0':
            item.result = 'Queueing'
        elif item.result == '1':
            item.result = 'Accepted'
        elif item.result == '2':
            item.result = 'Presentation Error'
        elif item.result == '3':
            item.result = 'Wrong Answer'
        elif item.result == '4':
            item.result = 'Time Limit Exceeded'
        elif item.result == '5':
            item.result = 'Memory Limit Exceeded'
        elif item.result == '6':
            item.result = 'Output Limit Exceeded'
        elif item.result == '7':
            item.result = 'Runtime Error'
        elif item.result == '8':
            item.result = 'Compilation Error'
        elif item.result == '9':
            item.result = 'Compile Output Limit'


#获取提交列表或者
class GetProblemSubmitPage(APIView):
    def get(self, request): #按（提交人|提交结果|提交语言|提交问题）查询第page页的提交列表
        submit_UserName = request.GET.get('submit_UserName', '')
        judge_States = request.GET.get('judege_States','')
        language = request.GET.get('language', '')
        problem_Id = request.GET.get('problem_Id', '')
        page_Num = request.GET.get('page', 1)
        query_Criteria = {}  # 创建一个多条件查询字典
        if submit_UserName != '':
            query_Criteria['user__username__iregex'] = submit_UserName #用户名不区分大小写
        if judge_States != '' and judge_States != '0':
            query_Criteria['result'] = judge_States
        if language != '' and language != 'All':  # 当值为C++时不知道为什么传不过来，推测可能是无法解析
            if language == 'Csrc':
                query_Criteria['language'] = 'C++'
            else:
                query_Criteria['language'] = language
        if problem_Id != '':
            try:
                query_Criteria['problem__no'] = int(problem_Id)
            except:
                pass
        submit_List = ProblemSubmit.objects.filter(**query_Criteria)
        submit_List = list(reversed(submit_List)) #反转 使最近提交在前
        change_status(submit_List)
        paginator_OfSubmitListAll = Paginator(submit_List, 25) #分页
        paginator_OfSubmitListAll = paginator_OfSubmitListAll.page(page_Num)
        paginator_OfSubmitListAll = paginator_OfSubmitListAll.object_list
        paginator_OfSubmitListAll = ProblemSubmitListSerializer(paginator_OfSubmitListAll, many=True).data
        data = {'status': 200, 'msg': '获取成功', 'data': {}}
        data['data'] = {
            'statuses': paginator_OfSubmitListAll, 'now_page': page_Num,
            'submit_user_name': submit_UserName, 'judge_states': judge_States,
            'language': language, 'problem_id': problem_Id
        }
        return JsonResponse(data)




#获取单个提交
class GetProblemSubmit(APIView):
    def get(self, request):
        data = {'status': 200, 'msg': '获取成功', 'data': {}}
        is_Login, user = check_auth(request._request)
        if is_Login == False:
            data["data"].update({'status': 400, 'msg': '未登录',})
            return JsonResponse(data)
        username = request.GET.get("username", "")
        if username != user.username:
            data["data"].update({'status': 400, 'msg': '不同用户', })
            return JsonResponse(data)
        problem_Submit_Id = request.GET.get("problem_Submit_Id","")
        print(problem_Submit_Id)
        problem_Submit = ProblemSubmit.objects.get(runID = problem_Submit_Id)
        problem_Submit = ProblemSubmitSerializer(problem_Submit).data
        data["data"] = {"problem_Submit":problem_Submit}
        return JsonResponse(data)


class CreateProblemSubmit(APIView):
    def post(self, request):  # 创一份ProblemSubmit
        data = {'status': 200, 'msg': '提交成功', 'data': {}}
        is_Login, user = check_auth(request._request)
        if is_Login == False:
            return JsonResponse(data.update({'status': 400, 'msg': '未登录'}))
        if request._request.is_ajax():

            prob_no = request.POST.get('prob_no', '')
            try:
                prob_no = int(prob_no)
            except:
                data['status'] = 400
                data['msg'] = 'Error: Unknown problem submit'
                return JsonResponse(data)
            if 1000 > prob_no or prob_no > Problem.objects.last().no:
                data['status'] = 400
                data['msg'] = 'Error: Unknown problem submit'
                return JsonResponse(data)
            # 对于防止恶意提交空语言， 进行判断提交语言是否符合规定
            language = request.POST.get('language', 'C')
            if not re.search('[C|C++|Java|Python]', language):
                language = 'C'

            # 获取提交代码
            content = request.POST.get('editor', '')

            if content != '':
                prob = Problem.objects.get(no=prob_no)
                statuses = ProblemSubmit.objects.create(
                    user=user,
                    problem=prob,
                    content=content,
                    result="0",
                    language=language,
                )
                prob.submit_nums += 1
                flag = 0
                while statuses.result == '0':
                    time.sleep(0.7)  # 如果还在判题，那就等0.7秒以后重新查询，直到结果出来后，再返回数据
                    if flag >= 10:
                        time.sleep(0.6)
                        if flag > 35:
                            data['status'] = 400
                            data['msg'] = 'Error: Server busy, try again later'
                            return JsonResponse(data)
                    statuses = ProblemSubmit.objects.filter(problem__no=prob_no, user__id=user.id).last()
                    flag += 1
                if statuses.result == '1':
                    prob.ac_nums += 1
                prob.ratio = round(float(prob.ac_nums / prob.submit_nums), 3) * 100
                prob.save()
                data['msg'] = 'Success: ' + str(statuses.result)
                return JsonResponse(data)
            else:
                # 对于空代码提交， 向ajax返回错误信息
                data['status'] = 400
                data['msg'] = 'Error: Your code is empty!'
                return JsonResponse(data)



