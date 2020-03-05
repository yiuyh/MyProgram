from django.contrib.auth import authenticate
from django.core.paginator import Paginator
from django.http import JsonResponse, HttpResponse
from django.views import View
from django.views.decorators.csrf import csrf_exempt
from rest_framework.response import Response

from UserProfile.util import *
from UserProfile.serializers import *
import re
from .Teacher import *
from .Administrator import *
from rest_framework.views import APIView

#获取用户信息或修改用户信息(登录用户)
class UserOwn(APIView):

    def get(self, request):
        print("xxxxxx")
        is_Login, user = check_auth(request._request) #获取登录的用户
        data = {'status': 200, 'msg': '成功获取用户信息','data':{}}
        if is_Login == True:
            data['data'] = user.get_user()
        else:
            data['msg'] = '未登录'
            data['status'] = 400
        return JsonResponse(data=data)

    def post(self, request):
        is_Login, user = check_auth(request._request)  # 获取登录的用户
        data = {'status': 200, 'msg': '修改成功', 'data': {}}
        if is_Login == True:
            user.updata_user(request.data)
        else:
            data['msg'] = '用户不存在'
            data['status'] = 400

        return JsonResponse(data=data)

#获取他人用户信息
class UserOther(APIView):
    def get(self, request, username):
        data = {'status': 200, 'msg': '成功获取用户信息', 'data': {}}
        try:
            user = User.objects.get(username=username)  # 获取不到会抛异常
            data['data'] = OtherUserSerializer(user).data
        except:
            data['msg'] = '用户不存在'
            data['status'] = 400
        return JsonResponse(data=data)


#获取token列表
class Tokens(View):
    def get(self, request):
        tokenList = UserToken.objects.all()
        userTokenSerializer = UserTokenSerializer(tokenList, many=True)
        data = {
            'msg': '已获取所有用户的Token',
            'data': userTokenSerializer.data
        }
        return JsonResponse(data=data)


#注册
class Register(APIView):

    def post(self, request):
        data = {'status': 200, 'msg': '注册成功', 'data': {}}  # 返回信息


        userPassword = request.data['password']
        userRepassword = request.data['repassword']
        userUsername = request.data['username']

        if userPassword != userRepassword: # 判断密码与确认密码是否相同
            data['status'] = 400  # 400 INVALID REQUEST - [POST/PUT/PATCH]：用户发出的请求有错误
            data['msg'] = '重复密码错误'
            return JsonResponse(data=data)
        elif not re.search(r'^[a-zA-Z_]\w{4,15}$', userUsername):# 判断用户名是否合法
            data['status'] = 400
            data['msg'] = '用户名只能包含字母数字下划线，不能以数字起头,长度不少于5'
        elif not re.search(r'^[0-9a-zA-Z]{8,16}$', userPassword): #判断密码是否合法
            data['status'] = 400
            data['msg'] = '密码由8-16位数字字母组成'
        elif User.objects.filter(username=userUsername).exists(): # 判断用户名是否已经存在
            data['status'] = 400
            data['msg'] = '用户名已存在'
        else:

            userSerializer = UserSerializer(data=request.data) #反序列化
            if userSerializer.is_valid(): #判断合法
                userSerializer,user = userSerializer.create()
                userJwt = jwt_response_payload_handler(user, request._request) #生成token
                usertoken = UserToken(user=user, token=userJwt['token']) #创建UserToken
                usertoken.save()
                print(len(userJwt['token']))
                data['data'] = {'username':userUsername,'token':userJwt['token']}

            # 上传头像
            # user_headImage = request.FILES.get('headImage')
            # user.headImage = user_headImage
            # user.save()
        return JsonResponse(data=data)

    def get(self, request):

        return HttpResponse("ok")

#登录
class Login(APIView):
    def post(self, request):
        data = data = {'status': 200, 'msg': '登录成功', 'data': {}}  # 返回信息
        username = request.data['username']
        password = request.data['password']
        print(username, password)
        if User.objects.filter(username=username).exists():
            user = authenticate(username=username, password=password) #Django内部的User认证系统
            if user:
                userJwt = jwt_response_payload_handler(user, request) #生成token
                user.change_token(userJwt['token']) #修改token
                data['data'] = {'username':username,'token':userJwt['token']}
            else:
                data['status'] = 400
                data['msg'] = '密码错误'
        else:
            data['status'] = 400
            data['msg'] = '该用户名不存在'
        return Response(data)


class Test(APIView):

    def get(self, request):
        # users = User.objects.all()
        # user_serializer = UserSerializer(users, many=True)
        #
        # return JsonResponse(user_serializer.data, safe=False)
        data = {"ok": "xx"}
        return JsonResponse(data)

    def post(self, request):
        # User.objects.create(**request.data)
        User.objects.create(username = request.data['username'],
                            password = request.data['password']
                            )
        # username = request.data['username']
        # user = User.objects.filter(username = username)
        # print(type(user))
        # user = user.first()
        # print(type(user))
        # usertoken = user.usertoken
        # print(usertoken.token)
        # usertoken_serializer = UserTokenSerializer(usertoken)
        # user_serializer = UserSerializer(user)
        return JsonResponse({'data1':request.data})


