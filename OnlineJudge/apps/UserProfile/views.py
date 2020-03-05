from django.shortcuts import render

from django.shortcuts import render,render_to_response
from django.http import HttpResponseRedirect,HttpResponse
from django.contrib import auth
from django.template import RequestContext
import time
from django.contrib.auth.models  import Group
from django.contrib import auth
from django.contrib.auth.models import User
from django.contrib.auth import authenticate
from django.contrib.auth.decorators import login_required
from django.http import HttpResponseRedirect
from UserProfile.models import User
from django.views.decorators.csrf import csrf_exempt
# 第四个是auth中用户权限有关的类。auth可以设置每个用户的权限。

from Contest.models import MatchSubmit,Match
from Issue.models import ProblemSubmit
from News.models import Info
from datetime import datetime,timedelta
from django.core.paginator import Paginator
# Create your views here.

# -*- coding: utf-8 -*-


def index(request):
    contest = Match.objects.filter(attribute='公开').order_by('-id')[:6]
    rank= User.objects.filter().order_by('-rating')[:10]
    newsinfo = Info.objects.filter().order_by('-subTime')[:5]
    return render(request, 'index.html', locals())


def userinfo_view(request, uid):
    try:
        user = User.objects.get(id=uid)
    except:
        return HttpResponseRedirect("/index/")
    user_name = user.username
    submit_time = ProblemSubmit.objects.filter(user__username=user_name).count()
    try_time = ProblemSubmit.objects.values('problem').distinct().count()
    right_time = ProblemSubmit.objects.filter(user__username=user_name, result=1).values('problem').distinct().count()
    return render(request, 'userinfo.html', locals())

# 注册


def register_view(req):
    if req.method == 'POST':
        user_pwd = req.POST.get("password", "")
        user_repwd = req.POST.get("repassword", "")
        if user_pwd != user_repwd:
            user_password_error = True
            return render(req, 'register.html', {'user_password_error': user_password_error})
        user_name = req.POST.get("username", "")
        if User.objects.filter(username=user_name).count():
            user_repeat = True
            return render(req, 'register.html', {'user_repeat': user_repeat})
        user_id = req.POST.get("stuId", "")
        user_school = req.POST.get("school", "")
        user_email = req.POST.get("email", "")
        user_tel = req.POST.get("tel", "")
        user = User.objects.create_user(username=user_name, password=user_pwd)
        user.school = user_school
        user.stuId = user_id
        user.email = user_email
        user.tel = user_tel
        user.save()
        req.session.flush()
        auth.login(req, user)
        Group.objects.get(name='普通用户').user_set.add(user)
        print(Group.objects.get(name='普通用户'))
        req.session.set_expiry(0)
        return HttpResponseRedirect('/index/')
    return render(req, 'register.html')

# 登陆


def login_view(req):
    if req.user.is_authenticated():
        return HttpResponseRedirect('/index/')
    if req.method == 'POST':
        username = req.POST.get('user_name')
        password = req.POST.get('user_password')
        user = authenticate(username=username, password=password)
        if user:
            req.session.flush()
            auth.login(req, user)
            if req.POST.get('remember') is None:
                req.session.set_expiry(0)
            else:
                req.session.set_expiry(timedelta(days=7))
            next_url = req.GET.get('next', '')
            if next_url != '':
               return HttpResponseRedirect(next_url)
            return HttpResponseRedirect('/index/')
        else:
            return render(req, 'login.html', {'user_error': True})
    return render(req, 'login.html', locals())

# 登出


@csrf_exempt
def logout_view(req):
    if req.user.is_authenticated():
        # 如果本来就未登录，也就没有登出一说
        auth.logout(req)
        req.session.flush()
    return HttpResponseRedirect("/login/")


def user_rank(request):
    submit_user_name = request.GET.get('submit_user_name', '')
    if submit_user_name != '':
        userrank = User.objects.filter(username__iregex='.*'.join(submit_user_name))
    else:
        userrank = User.objects.all().order_by('-rating', '-ac_nums')
    paginator_of_userrank_all = Paginator(userrank, 20)
    page_num = request.GET.get('page', 1)
    page_of_userrank_paginator = paginator_of_userrank_all.page(page_num)
    return render(request, 'rank.html', {'userrank': page_of_userrank_paginator, 'now_page': page_num, 'submit_user_name': submit_user_name})


def setting(request):
    if not request.user.is_authenticated():
        return HttpResponse('请先登录', 404)
    if request.method == 'POST':
        try:
            user = User.objects.get(id=str(request.session.get('user_id')))
        except:
            return HttpResponseRedirect("/index/")
        pwd = request.POST.get('pwd', '')
        repwd = request.POST.get('repwd', '')
        if pwd != repwd:
            return render(request, 'setting.html', {'setting_stu': 0})
        else:
            if pwd != '':
                user.set_password(str(pwd))
        email = request.POST.get('email', '')
        tel = request.POST.get('tel', '')
        synopsis = request.POST.get('synopsis', '')
        headImage = request.FILES.get('headImage', '')
        if email != '':
            user.email = email
        if tel != '':
            user.tel = tel
        if synopsis != '':
            user.synopsis = synopsis
        if headImage != '':
            user.headImage = headImage
        user.save()
        return render(request, 'setting.html', {'setting_stu': 1})

    return render(request, 'setting.html')
