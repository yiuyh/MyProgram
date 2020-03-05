from django.urls import path, re_path
from Api import views

urlpatterns_Issue = [

    path('issue/problems/', views.IssueApi.GetProblemPage.as_view(), name ='problem_page'),
    re_path(r'^issue/problem/(?P<prob_id>\d+)/$', views.IssueApi.GetProblem.as_view(), name='problem'),

    path('issue/problemsubmits/', views.IssueApi.GetProblemSubmitPage.as_view(), name ='problemsubmit_page'),
    path('issue/problemsubmit/', views.IssueApi.GetProblemSubmit.as_view(), name ='problemsubmit'),
]