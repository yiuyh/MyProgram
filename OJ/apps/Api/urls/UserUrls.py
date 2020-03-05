from django.urls import path, re_path
from Api import views

urlpatterns_User = [
    # 登录注册
    path('login/', views.Login.as_view(), name='login'),
    path('register/', views.Register.as_view(), name='register'),


    # 用户

    path('user/', views.UserOwn.as_view(), name='userown'),
    re_path(r'^user/(?P<username>[a-zA-Z0-9]+)/$', views.UserOther.as_view(), name='userother'),


    path('userlist/', views.UsersList.as_view(), name='userlist'),
    path('pwchange/', views.UsersPwChange.as_view(), name = 'pwchange'),


    path('tokens/', views.Tokens.as_view(), name='tokens'),
]