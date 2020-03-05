from django.http import HttpResponseRedirect, HttpResponse
from django.shortcuts import render
from .models import Match, MatchRegister, MatchSubmit, MatchRank
from Issue.models import Problem
from functools import cmp_to_key
import json, math, re, datetime, time, random
import threading
from django.http import JsonResponse
from datetime import datetime, timedelta
from django.views.decorators.csrf import csrf_exempt
from django.core.paginator import Paginator
from UserProfile.models import *
#encoding: utf-8
#指定编码,如果不指定无法进行中文字符的正则表达式匹配


def change_status(status_list):
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


def register_status(contests, user):
    for con in contests:
        time1 = con.startTime.replace(tzinfo=None)     # 比赛开始时间
        time2 = datetime.now()       # 当前系统时间
        time3 = time1+timedelta(minutes=int(con.howLong))     # 比赛结束时间
        flag_time = 0
        if time1 > time2:
            flag_time = 1
        elif time2 < time3 and time2 > time1:
            flag_time = 2
        elif time2 > time3:
            flag_time = 3
        flag = MatchRegister.objects.filter(match__id=con.id, user__username=user.username).count()
        con.reg_status = flag
        if flag_time <= 1:
            if flag == 0:
                Match.objects.filter(id=con.id).update(status=0)
                con.status = 0
            else:
                Match.objects.filter(id=con.id).update(status=1)
                con.status = 1
        else:
            Match.objects.filter(id=con.id).update(status=flag_time)
            con.status = flag_time


def show_contest(request):  # 比赛列表
    match_id_name = request.GET.get('match_id_name', '')  # 默认值为''
    query_criteria = {'attribute': '公开'}  # 创建一个多条件查询字典
    if match_id_name != '':
        try:
            query_criteria['id'] = int(match_id_name)
        except:
            query_criteria['matchName__regex'] = '.*'.join(match_id_name)
    contest = list(reversed(Match.objects.filter(**query_criteria)))
    register_status(contest, request.user)
    # 暂未优化
    try:
        match_status = int(request.GET.get('match_status', 0))
    except:
        match_status = 0
    cnt = []
    if match_status <= 3 and match_status >= 0:
        temp = []
        for con in contest:
            if con.status == match_status or (match_status == 1 and con.status == 0) or match_status == 0:
                temp.append(con)
        contest = temp
    paginator_of_contest_all = Paginator(contest, 10)
    page_num = request.GET.get('page', 1)
    paginator_page_of_contest = paginator_of_contest_all.page(page_num)
    ret_dir = {'contest': paginator_page_of_contest,
               'now_page': page_num, 'match_status': match_status,
               'match_id_name': match_id_name}
    if request.GET.get('alr_reg', '') != '':
        ret_dir['alr_reg'] = 'You have registered for the competition.'
    reg_suc = request.GET.get('reg_suc', '')
    if reg_suc != '':
        if reg_suc == '1':
            ret_dir['reg_info'] = 'registered successfully.'
        elif reg_suc == '0':
            ret_dir['reg_info'] = 'The registration time has passed.'
    return render(request, 'contestList.html', ret_dir)


def ratio(match, problems):  # 通过率
    ratio_list = []
    for prob in problems:
        nums = MatchSubmit.objects.filter(match__id=match.id, problem__no=prob.no).exclude(result='0')
        ac_nums = nums.filter(result='1').count()
        nums = len(nums)
        if ac_nums != 0:
            ratio_list.append(round(float(ac_nums/nums), 3) * 100)
        else:
            ratio_list.append('0')
    return ratio_list


def problem_ac_nums(match, problems):  # AC nums
    ac_list = []
    for prob in problems:
        nums = MatchSubmit.objects.filter(match__id=match.id, problem__no=prob.no).exclude(result='0')
        ac_nums = nums.filter(result='1')
        ac_nums = len(ac_nums)
        ac_list.append(ac_nums)
        HttpResponse
    return ac_list


def problem_submit_nums(match, problems):  # submit nums
    submit_list = []
    for prob in problems:
        nums = MatchSubmit.objects.filter(match__id=match.id, problem__no=prob.no).exclude(result='0')
        nums = len(nums)
        submit_list.append(nums)
    return submit_list


def is_accepted(match, user, probs):
    ac_list = []
    for prob in probs:
        if MatchSubmit.objects.filter(match__id=match.id, user__username=user.username, problem__no=prob.no, result='1').exists():
            ac_list.append('yes')
        else:
            ac_list.append('')
    return ac_list


# 以下为展示比赛内容
def contest_show_content(request, match_id):  # 比赛包含的题目
    try:
        contest = Match.objects.get(id=match_id)
    except:
        return HttpResponseRedirect('/contest/match/')
    start_time = contest.startTime
    start_time = {'year': start_time.year, 'month': start_time.month, 'day': start_time.day,
                  'hour': start_time.hour, 'minute': start_time.minute}
    length = contest.howLong
    problems = list(contest.include.all())
    time1 = contest.startTime.replace(tzinfo=None)  # 比赛开始时间
    time2 = datetime.now()  # 当前系统时间
    ret_dir = {
        'startTime': start_time,
        'length': length,
        'contest_name': contest.matchName,
        'contest_id': contest.id
    }
    if time2 < time1:
        ret_dir['is_start'] = False
    elif time2 >= time1:
        ret_dir['contest_info'] = contest.info
        ret_dir['contest_problems'] = problems
        ret_dir['is_start'] = True
    return render(request, 'contest.html', ret_dir)


# 以下为展示比赛题目, 此函数是由Ajax提交的
def contest_get_problems(request, match_id):
    if request.is_ajax():
        try:
            contest = Match.objects.get(id=match_id)
            # 尝试获取比赛，获取不到就不做处理
        except:
            return
        problems = list(contest.include.all())
        ratio_list = ratio(contest, problems)
        ac_nums = problem_ac_nums(contest, problems)
        submit_nums = problem_submit_nums(contest, problems)
        ac_list = is_accepted(contest, request.user, problems)
        ret_dir = {
            'ratio_list': ratio_list,
            'ac_nums': ac_nums,
            'submit_nums': submit_nums,
            'ac_list': ac_list
        }
    return HttpResponse(json.dumps({'Success': ret_dir}))


# 以下为展示比赛排名, 此函数是由Ajax提交的

def contest_get_rank(request, match_id):
    if request.is_ajax():
        try:
            contest = Match.objects.get(id=match_id)  # 获得这场比赛的所有信息
        except:
            return HttpResponse(json.dumps({'Error': {'content': 'No such contest!'}}))
        # 从比赛中获取题目列表
        contest_name = contest.matchName
        problem_id_list = list(contest.include.all())
        users = MatchRegister.objects.filter(match__id=contest.id)  # 从注册比赛的所有用户中查找用户
        if not users.exists():
            return HttpResponse(json.dumps({'Error': {'content': 'No user registration contest!'}}))
        return HttpResponse(json.dumps({'Success': {'rankList': []}}))



# 以下为展示比赛状态, 此函数是由Ajax提交的
# 做一个题目顺序与序号的映射表
PROBLEMS_LIST = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
      'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z']


def contest_get_status(request, match_id):
    if request.is_ajax():
        try:
            contest = Match.objects.get(id=match_id)
        except:
            return HttpResponse(json.dumps({'Error': {'content': 'No such contest!'}}))

        # 从提交GET 提交请求中去获取筛选条件
        submit_user_name = request.GET.get('submit_user_name', '')
        page_num = request.GET.get('page', 1)
        judge_states = request.GET.get('judge_states', '')
        language = request.GET.get('language', '')
        problem_id = request.GET.get('problem_id', '')

        query_criteria = {'match__id': contest.id}  # 创建一个多条件查询字典

        # 创建对用户名进行正则匹配的条件
        if submit_user_name != '':
            query_criteria['user__username__iregex'] = submit_user_name

        # 创建对题目状态尽行筛选的条件
        if judge_states != '' and judge_states != '0':
            query_criteria['result'] = judge_states

        # 创建对提交语言进行筛选的条件
        if language != '' and language != 'All':  # 当值为C++时不知道为什么传不过来，推测可能是无法解析
            if language == 'Csrc':
                query_criteria['language'] = 'C++'
            else:
                query_criteria['language'] = language

        # 创建对题目的筛选条件
        if problem_id != '' and problem_id != '0':
            try:
                query_criteria['problem__no'] = int(problem_id)
            except:
                pass

        temp = MatchSubmit.objects.filter(**query_criteria).order_by('-runID')
        # change_status(temp) 放弃在后台进行状态转化是因为在前端需要利用状态数字编号来进行分别颜色

        # 获取比赛题目列表
        problems_list = list(contest.include.all())

        # 由于在将查询到的statusList直接进行json转化时，发生类型不匹配，推测可能是时json.dumps直直刺基本数据类型转化
        # 故将查询到的statusList 遍历一遍封装为list

        # 分页处理
        paginator_of_status_all = Paginator(temp, 30)
        page_of_status_paginator = paginator_of_status_all.page(page_num)

        # 创建一个返回状态列表
        statusList = []
        for item in page_of_status_paginator:
            _temp = {}
            _temp['contest_id'] = contest.id
            _temp['userName'] = item.user.username
            _temp['uid'] = item.user.id
            _temp['probID'] = item.problem.no
            _temp['probName'] = PROBLEMS_LIST[problems_list.index(item.problem)]
            _temp['runID'] = item.runID
            _temp['result'] = item.result
            _temp['time'] = item.time
            _temp['memory'] = item.memory
            _temp['language'] = item.language
            _temp['subTime'] = item.subTime.strftime('%Y-%m-%d %H:%M:%S')
            statusList.append(_temp)
        ret_dir = {
            'statusList': statusList, 'now_page': page_num, 'page_num': paginator_of_status_all.num_pages,
            'submit_user_name': submit_user_name, 'judge_states': judge_states,
            'language': language, 'problem_id': problem_id
        }
        return HttpResponse(json.dumps({'Success': ret_dir}))


# 以下为在比赛页面进行提交代码, 此函数是由Ajax提交的
@csrf_exempt
def contest_submit_status(request):
    if request.method == 'POST':
        if request.is_ajax():
            contest_id = request.POST.get('contest_id', '')
            contest = Match.objects.filter(id=contest_id)
            if contest.exists():
                contest = contest[0]
            else:
                return
            # 获取时间 进行是否还在比赛的判定
            time1 = contest.startTime.replace(tzinfo=None)  # 比赛开始时间
            time2 = datetime.now()  # 当前系统时间
            time3 = time1 + timedelta(minutes=int(contest.howLong))  # 比赛结束时间

            # 如果还未开始， 就重定向到比赛开始页面
            if time1 > time2:
                return HttpResponseRedirect('/contest/match/'+str(contest_id) + '/')
            elif time2 > time3:
                # 如果比赛已经结束， 向前端ajax提交处返回错误信息
                return HttpResponse(json.dumps({'Error': {'result': 'contest is over'}}))

            # 对于非法提交， 进行判断该用户是否注册了比赛， 没有就重定向到比赛列表页面
            if not MatchRegister.objects.filter(match__id=contest_id, user__username=request.user.username).exists():
                return HttpResponseRedirect('/contest/match/')
            prob_no = request.POST.get('prob_no', '')
            if contest_id == '' or prob_no == '':
                return HttpResponseRedirect('/contest/match/')

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
                    user=request.user,
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
                            return HttpResponse(json.dumps({'Error': {'result': 'Server busy, try again later'}}))
                    statuses = MatchSubmit.objects.filter(problem__no=prob_no, user__username=request.user.username).last()
                    flag += 1
                return HttpResponse(json.dumps({'Success': {'result': statuses.result}}))
            else:
                # 对于空代码提交， 向ajax返回错误信息
                return HttpResponse(json.dumps({'Error': {'result': 'Your code is empty!'}}))


# 以下为注册比赛
def register_contest(request, match_id):                                # 比赛注册
    match = Match.objects.get(id=match_id)
    time1 = match.startTime.replace(tzinfo=None)  # 比赛开始时间
    time2 = datetime.now()  # 当前系统时间
    time3 = time1 + timedelta(minutes=int(match.howLong))  # 比赛结束时间
    if time1 > time2:
        if MatchRegister.objects.filter(match__id=match.id, user__id=request.user.id).exists():
            return HttpResponseRedirect("/contest/match/?alr_reg=1")
        MatchRegister.objects.create(match=match,  user=request.user)
        match.registerNum += 1
        match.save()
        return HttpResponseRedirect("/contest/match/?reg_suc=1")
    elif time2 < time3 and time2 > time1:
        return HttpResponseRedirect("/contest/match/?reg_suc=0")
    elif time2 > time3:
        return HttpResponseRedirect("/contest/match/?reg_suc=0")


'''
    先通过比赛名称查找出注册比赛的用户和比赛包含的题目，然后根据题目去查找用户的A题个数，最后整合后排序
'''


@csrf_exempt
def contest_problem_page(request, match_id, prob_id):    # 题目列表
    problem = Problem.objects.filter(no=prob_id)
    if not problem.exists():
        return
    else:
        problem = problem[0]
    t = MatchSubmit.objects.filter(problem__no=problem.no, match__id=match_id, user__id=request.user.id).last()
    code = ''
    language = 'C'
    if t:
        language = t.language
        code = t.content
    no = request.GET.get('Problem', '')
    return render(request, 'contest_problem.html', {'problem': problem, 'code': code,'contest_id': match_id, 'no': no,
                                                    'language': language})


def contest_register_list(request, match_id):
    r = MatchRegister.objects.filter(match__id=match_id)
    result_list = []
    for i in r:
        result_list.append(i.user)
    sort_list = sorted(result_list, key=lambda x: (-x.rating))
    return render(request, 'contestRegister.html', {'sort_list': sort_list})


def code(request, match_id, run_id):
    if not request.user.is_authenticated():
        return HttpResponse('请先登录', 404)
    submit = MatchSubmit.objects.filter(runID=int(run_id))
    if not submit.exists():
        return
    submit = submit[0]
    if request.user != submit.user:
        return HttpResponseRedirect('/contest/match/')
    return render(request, 'code.html', {'submit': submit})