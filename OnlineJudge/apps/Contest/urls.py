from django.conf.urls import url
from Contest import views
app_name = 'contest'

urlpatterns = [
    url(r'^submit_status/$', views.contest_submit_status, name='submit_status'),
    url(r'^match/$', views.show_contest, name='match'),
    url(r'^(?P<match_id>\d+)/$', views.contest_show_content, name='contest_content'),
    url(r'^rank/(?P<match_id>\d+)/$', views.contest_get_rank, name='contestRank'),
    url(r'^problems/(?P<match_id>\d+)/$', views.contest_get_problems, name='contestProblems'),
    url(r'^contest_status/(?P<match_id>\d+)/$', views.contest_get_status, name='contestStatus'),
    url(r'^register_contest/(?P<match_id>\d+)/$', views.register_contest, name='register_contest'),
    url(r'^(?P<match_id>\d+)/(?P<prob_id>\d+)/$', views.contest_problem_page, name='contest_problem_page'),
    url(r'^matchRegisterList/(?P<match_id>\d+)/$', views.contest_register_list, name='contestRegister'),
    url(r'^(?P<match_id>\d+)/code/(?P<run_id>\d+)/$', views.code, name='code'),
]
