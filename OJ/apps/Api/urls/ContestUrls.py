from django.urls import path, re_path
from Api import views

urlpatterns_Contest = [
    path('contest/contests/', views.ContestApi.GetContestPage.as_view(), name ='contests'),

    re_path(r'^contest/problems/(?P<match_id>\d+)/$', views.ContestShowContent.as_view(), name='contest_pro'),
    path('test/', views.UserApi.Test.as_view(), name ='test'),
]