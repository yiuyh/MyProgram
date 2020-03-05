from django.shortcuts import render
from .models import Problem, ProblemSubmit
from Contest.models import MatchRegister, Match, MatchSubmit
from django.http import HttpResponseRedirect, HttpResponse
from django.views.decorators.csrf import csrf_exempt
import json, time, random, re
from django.core.paginator import Paginator


# _*_ coding: utf-8 _*_

def is_accepted(user, probs):
    ac_list = []
    for prob in probs:
        if ProblemSubmit.objects.filter(user__id=user.id, problem__no=prob.no, result='1').exists():
            ac_list.append('yes')
        else:
            ac_list.append('')
    return ac_list


def problem_page(request, prob_id):    # 题目列表
    if request.method == "GET":
        if str(prob_id) == '0':
            problem_id_name = request.GET.get('problem_id_name', '')
            problem_source = request.GET.get('problem_source', 'All')
            algorithm_type = request.GET.get('algorithm_type', 'All')
            query_criteria = {}  # 创建一个多条件查询字典
            if problem_id_name != '':
                try:  # 尝试将参数作为数值处理，并当作id搜索，如果不行，就转化为name进行搜索
                    query_criteria['no'] = int(problem_id_name)
                except:
                    query_criteria['title__iregex'] = '.*'.join(problem_id_name)
            if problem_source != '' and problem_source != 'All':
                query_criteria['probSource'] = problem_source
            if algorithm_type != '' and algorithm_type != 'All':
                query_criteria['classification'] = algorithm_type
            problems = Problem.objects.filter(**query_criteria)
            #分页处理
	    
            paginator_of_problems_all = Paginator(list(problems), 20)
            page_num = request.GET.get('page', 1)
            page_of_problems_paginator = paginator_of_problems_all.page(page_num)
            ac_flag = is_accepted(request.user, page_of_problems_paginator)
            # 将这些数据返回到前端，这样便于进行多条件过滤
            ret_dir = {'problems': page_of_problems_paginator, 'ac_flag': ac_flag,
                       'now_page': page_num, 'problem_id_name': problem_id_name,
                       'problem_source': problem_source, 'algorithm_type': algorithm_type}
            return render(request, 'problemList.html', ret_dir)
        else:
            prob_no = str(prob_id)
            problem = Problem.objects.filter(no=prob_no, probAuthority='公开')
            r = ProblemSubmit.objects.filter(problem__no=prob_no, user__id=request.user.id).last()
            code = ''
            language = 'C'
            if r:
                language = r.language
                code = r.content
            if problem.exists():
                problem = problem[0]
            else:
                return HttpResponseRedirect('/problem/page/0/')
            return render(request, 'issue_problem.html', {'problem': problem, 'code': code, 'language': language})


def change_status(status_list):  # 提交状态
    for item in status_list:
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


# post: 0表示提交代码 1表示查询数据
@csrf_exempt
def problem_status(request):  # 提交状态
    if request.method == 'POST':
        if not request.user.is_authenticated():
            return HttpResponse('请先登录', 404)
        if request.is_ajax():
            prob_no = request.POST.get('prob_no', '')
            try:
                prob_no = int(prob_no)
            except:
                return HttpResponse(json.dumps({'Error': {'result': 'Unknown problem submit'}}))
            if 1000 > prob_no or prob_no > Problem.objects.last().no:
                return HttpResponse(json.dumps({'Error': {'result': 'Unknown problem submit'}}))
            # 对于防止恶意提交空语言， 进行判断提交语言是否符合规定
            language = request.POST.get('language', 'C')
            if not re.search('[C|C++|Java|Python]', language):
                language = 'C'

            # 获取提交代码
            content = request.POST.get('editor', '')

            if content != '':
                p = Problem.objects.get(no=prob_no)
                statuses =ProblemSubmit.objects.create(
                    user=request.user,
                    problem=p,
                    content=content,
                    result="0",
                    language=language,
                )
                p.submit_nums += 1
                flag = 0
                while statuses.result == '0':
                    time.sleep(0.7)  # 如果还在判题，那就等0.7秒以后重新查询，直到结果出来后，再返回数据
                    if flag >= 10:
                        time.sleep(0.6)
                        if flag > 35:
                            return HttpResponse(json.dumps({'Error': {'result': 'Server busy, try again later'}}))
                    statuses = ProblemSubmit.objects.filter(problem__no=prob_no, user__id=request.user.id).last()
                    flag += 1
                if statuses.result == '1':
                    p.ac_nums += 1
                p.ratio = round(float(p.ac_nums/p.submit_nums), 3) * 100
                p.save()
                return HttpResponse(json.dumps({'Success': {'result': statuses.result}}))
            else:
                # 对于空代码提交， 向ajax返回错误信息
                return HttpResponse(json.dumps({'Error': {'result': 'Your code is empty!'}}))
    else:
        submit_user_name = request.GET.get('submit_user_name', '')
        page_num = request.GET.get('page', 1)
        judge_states = request.GET.get('judge_states', '')
        language = request.GET.get('language', '')
        problem_id = request.GET.get('problem_id', '')
        query_criteria = {}  # 创建一个多条件查询字典
        if submit_user_name != '':
            query_criteria['user__username__iregex'] = submit_user_name
        if judge_states != '' and judge_states != '0':
            query_criteria['result'] = judge_states
        if language != '' and language != 'All':  # 当值为C++时不知道为什么传不过来，推测可能是无法解析
            if language == 'Csrc':
                query_criteria['language'] = 'C++'
            else:
                query_criteria['language'] = language
        if problem_id != '':
            try:
                query_criteria['problem__no'] = int(problem_id)
            except:
                pass
        statuses = ProblemSubmit.objects.filter(**query_criteria)
        statuses = list(reversed(statuses))
        change_status(statuses)
        paginator_of_statuses_all = Paginator(statuses, 25)
        page_of_statuses_paginator = paginator_of_statuses_all.page(page_num)
        ret_dir = {'statuses': page_of_statuses_paginator, 'now_page': page_num,
                   'submit_user_name': submit_user_name, 'judge_states': judge_states,
                   'language': language, 'problem_id': problem_id }
        return render(request, 'status.html', ret_dir)


def code(request, run_id):
    if not request.user.is_authenticated():
        return HttpResponse('请先登录', 404)
    submit = ProblemSubmit.objects.filter(runID=run_id)
    if not submit.exists():
        return HttpResponseRedirect('/problem/problem_status/')
    submit = submit[0]
    if request.user != submit.user:
        return HttpResponseRedirect('/problem/problem_status/')
    return render(request, 'code.html', {'submit': submit})
