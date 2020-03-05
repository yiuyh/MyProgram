import json, math, re, time, random
from datetime import datetime, timedelta

from django.core.paginator import Paginator
from django.http import JsonResponse
from django.views import View
from rest_framework.views import APIView
from Contest.serializers import *

from Contest.models import *
from UserProfile.util import check_auth



def change_status(status_List):
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

def register_status(contests, user):
    for con in contests:
        time1 = con.startTime.replace(tzinfo=None)     # 比赛开始时间
        time2 = datetime.now()       # 当前系统时间
        time3 = time1 + timedelta(minutes=int(con.howLong))     # 比赛结束时间
        flag_time = 0
        if time1 > time2: #比赛未开始
            flag_time = 1
        elif time2 < time3 and time2 > time1: #比赛进行中
            flag_time = 2
        elif time2 > time3: #比赛已结束
            flag_time = 3
        try:
            flag =MatchRegister.objects.filter(match__id=con.id, user__username=user.username).count()
        except:
            flag = 0
        con.reg_status = flag #动态成员  用户对该比赛注册状态

        Match.objects.filter(id=con.id).update(status=flag_time)
        con.status = flag_time




class GetContestPage(APIView):

    def get(self, request): #按（id|名字|状态）查询第page页的竞赛列表
        data = {'status': 200, 'msg': '成功获取竞赛列表', 'data': {}}
        match_Id_Name = request.GET.get('match_id_name', '')  # 默认值为''
        query_Criteria = {'attribute': '公开'}  # 创建一个多条件查询字典
        if match_Id_Name != '':
            try:
                query_Criteria['id'] = int(match_Id_Name)
            except:
                query_Criteria['matchName__regex'] = '.*'.join(match_Id_Name)
        contest = list(reversed(Match.objects.filter(**query_Criteria)))

        is_Logion, user = check_auth(request._request)
        register_status(contest, user) #判断用户对竞赛列表的注册情况
        # 暂未优化
        try:
            match_Status = int(request.GET.get('match_status', 0))
        except:
            match_Status = 0

        if match_Status <= 3 and match_Status >= 0:
            temp = []
            for con in contest:
                if con.status == match_Status or (match_Status == 1 and con.status == 0) or match_Status == 0:
                    temp.append(con)
            contest = temp

        #竞赛列表分页&序列化
        paginator_OfContestAll = Paginator(contest, 10)
        page_num = request.GET.get('page', 1)
        paginator_OfContestAll = paginator_OfContestAll.page(page_num)
        paginator_OfContestAll = paginator_OfContestAll.object_list
        paginator_OfContestAll = MatchSerializer(paginator_OfContestAll, many=True).data

        data["data"].update({
            'contest': paginator_OfContestAll,
            'now_page': page_num, 'match_status': match_Status,
            'match_id_name': match_Id_Name})

        # if request.GET.get('alr_reg', '') != '':
        #     ret_dir['alr_reg'] = 'You have registered for the competition.'
        # reg_suc = request.GET.get('reg_suc', '')
        # if reg_suc != '':
        #     if reg_suc == '1':
        #         ret_dir['reg_info'] = 'registered successfully.'
        #     elif reg_suc == '0':
        #         ret_dir['reg_info'] = 'The registration time has passed.'
        return JsonResponse(data)


# 通过率
def ratio(match, problems):
    ratio_List = []
    for prob in problems:
        nums = MatchSubmit.objects.filter(match__id=match.id, problem__no=prob.no).exclude(result='0')
        ac_Nums = nums.filter(result='1').count()
        nums = len(nums)
        if ac_Nums != 0:
            ratio_List.append(round(float(ac_Nums/nums), 3) * 100)
        else:
            ratio_List.append('0')
    return ratio_List


# AC nums
def problem_ac_nums(match, problems):
    ac_List = []
    for prob in problems:
        nums = MatchSubmit.objects.filter(match__id=match.id, problem__no=prob.proNo).exclude(result='0')
        ac_Nums = nums.filter(result='1')
        ac_Nums = len(ac_Nums)
        ac_List.append(ac_Nums)
    return ac_List


# submit nums
def problem_submit_nums(match, problems):
    submit_List = []
    for prob in problems:
        nums = MatchSubmit.objects.filter(match__id=match.id, problem__no=prob.proNo).exclude(result='0')
        nums = len(nums)
        submit_List.append(nums)
    return submit_List


def is_accepted(match, user, probs):
    ac_List = []
    for prob in probs:
        try:
            if MatchSubmit.objects.filter(match__id=match.id, user__username=user.username, problem__no=prob.proNo, result='1').exists():
                ac_List.append('yes')
            elif MatchSubmit.objects.filter(match__id=match.id, user__username=user.username, problem__no=prob.proNo).exists():
                ac_List.append('no')
            else:
                ac_List.append('')
        except:
            ac_List.append('')
    return ac_List


# 以下为展示比赛内容

class ContestShowContent(APIView):
    def get(self, request, match_id):   # 比赛包含的题目
        data = {'status': 200, 'msg': '成功获取比赛题目列表', 'data': {}}
        try:
            contest = Match.objects.get(id=match_id)
        except:
            data.update({'status': 400, 'msg': '竞赛不存在'})
            return JsonResponse(data)

        start_Time = contest.startTime
        start_Time = {'year': start_Time.year, 'month': start_Time.month, 'day': start_Time.day,
                      'hour': start_Time.hour, 'minute': start_Time.minute}
        length = contest.howLong
        problems = MatchIncludeSerializer(contest.matchinclude_set.all(), many = True).data
        time1 = contest.startTime.replace(tzinfo=None)  # 比赛开始时间
        time2 = datetime.now()  # 当前系统时间
        data["data"].update({
            'startTime': start_Time,
            'length': length,
            'contest_name': contest.matchName,
            'contest_id': contest.id})
        if time2 < time1:
            data["data"].update({'is_start':False})
        elif time2 >= time1:
            data["data"].update({
                'contest_info':contest.info,
                'contest_problems':problems,
                'is_start':True})
        return JsonResponse(data)


# 以下为展示比赛题目, 此函数是由Ajax提交的
class ContestGetProblems(APIView):
    def get(self, request, match_id): #获取题目的提交人数&通过人数&通过率&用户的通过情况
        data = {'status': 200, 'msg': '成功获取比赛题目列表', 'data': {}}
        if request._request.is_ajax():
            try:
                contest = Match.objects.get(id=match_id)
                # 尝试获取比赛，获取不到就不做处理
            except:
                data.update({'status': 400, 'msg': '竞赛不存在'})
                return JsonResponse(data)
            problems = MatchIncludeSerializer(contest.matchinclude_set.all, many = True).data
            ratio_List = ratio(contest, problems) #通过率
            ac_Nums = problem_ac_nums(contest, problems) #通过人数
            submit_Nums = problem_submit_nums(contest, problems) #提交人数
            is_Login, user = check_auth(request._request)
            ac_List = is_accepted(contest, user, problems) #用户通过情况
            data["data"] = {
                'ratio_list': ratio_List,
                'ac_nums': ac_Nums,
                'submit_nums': submit_Nums,
                'ac_list': ac_List
            }
        return JsonResponse(data)


# 以下为展示比赛排名, 此函数是由Ajax提交的

# class ContestGetRank(APIView):
#     def get(self, request, match_id):
#         data = {'status': 200, 'msg': '获取成功', 'data': {}}
#         if request._request.is_ajax():
#             try:
#                 contest = Match.objects.get(id=match_id)  # 获得这场比赛的所有信息
#             except:
#                 data.update({'msg':'Error: No such contest!'})
#                 return JsonResponse(data)
#             # 从比赛中获取题目列表
#             contest_name = contest.matchName
#             problem_id_list = list(contest.include.all())
#             users = MatchRegister.objects.filter(match__id=contest.id)  # 从注册比赛的所有用户中查找用户
#             if not users.exists():
#                 return HttpResponse(json.dumps({'Error': {'content': 'No user registration contest!'}}))
#             return HttpResponse(json.dumps({'Success': {'rankList': []}}))



# 以下为展示比赛状态, 此函数是由Ajax提交的
# 做一个题目顺序与序号的映射表
PROBLEMS_LIST = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
      'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z']


class ContestGetStatus(APIView):
    def get(self, request, match_id):
        data = {'status': 200, 'msg': '获取成功', 'data': {}}
        if request._request.is_ajax():
            try:
                contest = Match.objects.get(id=match_id)
            except:
                data.update({'msg': 'Error: No such contest!'})
                return JsonResponse(data)

            # 从提交GET 提交请求中去获取筛选条件
            submit_User_Name = request.GET.get('submit_User_Name', '')
            page_Num = request.GET.get('page', 1)
            judge_States = request.GET.get('judge_States', '')
            language = request.GET.get('language', '')
            problem_Id = request.GET.get('problem_Id', '')

            query_criteria = {'match__id': contest.id}  # 创建一个多条件查询字典

            # 创建对用户名进行正则匹配的条件
            if submit_User_Name != '':
                query_criteria['user__username__iregex'] = submit_User_Name

            # 创建对题目状态尽行筛选的条件
            if judge_States != '' and judge_States != '0':
                query_criteria['result'] = judge_States

            # 创建对提交语言进行筛选的条件
            if language != '' and language != 'All':  # 当值为C++时不知道为什么传不过来，推测可能是无法解析
                if language == 'Csrc':
                    query_criteria['language'] = 'C++'
                else:
                    query_criteria['language'] = language

            # 创建对题目的筛选条件
            if problem_Id != '' and problem_Id != '0':
                try:
                    query_criteria['problem__no'] = int(problem_Id)
                except:
                    pass

            matchSubmit_List = MatchSubmit.objects.filter(**query_criteria).order_by('-runID')
            # change_status(temp) 放弃在后台进行状态转化是因为在前端需要利用状态数字编号来进行分别颜色

            # 分页处理
            paginator_OfStatusAll = Paginator(matchSubmit_List, 30)
            matchSubmit_List = paginator_OfStatusAll.page(page_Num).object_list
            matchSubmit_List = MatchSubmitListSerializer(matchSubmit_List, many=True).data

            # 创建一个返回状态列表
            # statusList = []
            # for item in page_of_status_paginator:
            #     _temp = {}
            #     _temp['contest_id'] = contest.id
            #     _temp['userName'] = item.user.username
            #     _temp['uid'] = item.user.id
            #     _temp['probID'] = item.problem.no
            #     _temp['probName'] = PROBLEMS_LIST[problems_list.index(item.problem)]
            #     _temp['runID'] = item.runID
            #     _temp['result'] = item.result
            #     _temp['time'] = item.time
            #     _temp['memory'] = item.memory
            #     _temp['language'] = item.language
            #     _temp['subTime'] = item.subTime.strftime('%Y-%m-%d %H:%M:%S')
            #     statusList.append(_temp)
            data["data"] = {
                'statusList': matchSubmit_List, 'now_page': page_Num, 'page_num': paginator_OfStatusAll.num_pages,
                'submit_user_name': submit_User_Name, 'judge_states': judge_States,
                'language': language, 'problem_id': problem_Id
            }
            return JsonResponse(data)


# 以下为在比赛页面进行提交代码, 此函数是由Ajax提交的
class contestSubmitStatus(APIView):
    def post(self, request):
        data = {'status': 200, 'msg': '获取成功', 'data': {}}
        is_Login, user = check_auth(request._request)
        if is_Login == False:
            data.update({'status': 400, 'msg': '未登录'})
            return JsonResponse(data)
        if request._request.is_ajax():
            contest_Id = request.POST.get('contest_Id', '')
            contest = Match.objects.filter(id=contest_Id)
            if contest.exists():
                contest = contest[0]
            else:
                data.update({'status': 400, 'msg': '竞赛不存在'})
                return JsonResponse(data)
            # 获取时间 进行是否还在比赛的判定
            time1 = contest.startTime.replace(tzinfo=None)  # 比赛开始时间
            time2 = datetime.now()  # 当前系统时间
            time3 = time1 + timedelta(minutes=int(contest.howLong))  # 比赛结束时间

            # 如果还未开始， 就重定向到比赛开始页面
            if time1 > time2:
                data.update({'status': 400, 'msg': 'Error： contest not start'})
                return JsonResponse(data)
            elif time2 > time3:
                # 如果比赛已经结束， 向前端ajax提交处返回错误信息
                data.update({'status': 400, 'msg': 'Error： contest is over'})
                return JsonResponse(data)

            # 对于非法提交， 进行判断该用户是否注册了比赛， 没有就重定向到比赛列表页面
            if not MatchRegister.objects.filter(match__id=contest_Id, user__username=user.username).exists():
                data.update({'status': 400, 'msg': '未注册比赛'})
                return JsonResponse(data)

            prob_no = request.POST.get('prob_no', '')

            if contest_Id == '' or prob_no == '':
                data.update({'status': 400, 'msg': '未知错误'})
                return JsonResponse(data)

            # 对于防止恶意提交空语言， 进行判断提交语言是否符合规定
            language = request.POST.get('language', 'C')
            if not re.search('[C|C++|Java|Python]', language):
                language = 'C'

            # 获取提交代码
            content = request.POST.get('editor', '')

            if content != '':
                # 创建提交记录
                statuses = MatchSubmit.objects.create(
                    match=contest,
                    user=user,
                    problem=Problem.objects.get(no=int(prob_no)),
                    content=content,
                    result="0",
                    language=language,
                )
                flag = 1
                # 进行循环查询题目结果
                while statuses.result == '0':
                    time.sleep(1)  # 如果还在判题，那就等一秒以后重新查询，直到结果出来后，再返回数据
                    if flag > 10:
                        time.sleep(1)
                        if flag > 35:
                            data.update({'status': 400, 'msg': 'Error： Server busy, try again later'})
                            return JsonResponse(data)
                    statuses = MatchSubmit.objects.filter(problem__no=prob_no, user__username=user.username).last()
                    flag += 1
                data["data"].update({'result': statuses.result})
                return JsonResponse(data)
            else:
                # 对于空代码提交， 向ajax返回错误信息
                data.update({'status': 400, 'msg': 'Error： Your code is empty!'})
                return JsonResponse(data)


