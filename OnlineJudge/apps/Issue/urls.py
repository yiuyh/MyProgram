from django.conf.urls import url
from . import views
app_name = 'problem'

urlpatterns = [
    url(r'^page/(?P<prob_id>\d+)/$', views.problem_page, name='problem_page'),
    url(r'^problem_status/$', views.problem_status, name='problem_status'),
    url(r'^code/(?P<run_id>\d+)/$', views.code, name='code'),
]
