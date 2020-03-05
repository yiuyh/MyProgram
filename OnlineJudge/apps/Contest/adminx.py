import xadmin
from Contest.models import *


class MatchAdmin():

    def get_submit(self, obj):
        submits = obj.matchregister_set.all()
        return ''
    list_display = ['matchName', 'startTime', 'howLong', 'registerNum', 'owner', 'get_submit']
    style_fields = {'content': 'ueditor'}
    menu_style = "accordion"

class MatchSubmitAdmin():

    def get_match_name(self, obj):
        return obj.match.matchName
    def get_user_name(self, obj):
        return obj.user.username
    def get_problem_id(self, obj):
        return obj.problem.no
    list_display = ['runID', "get_match_name", "get_user_name", "get_problem_id", 'result']
    style_fields = {'content': 'ueditor'}
    menu_style = "accordion"

xadmin.site.register(Match, MatchAdmin)
xadmin.site.register(MatchSubmit, MatchSubmitAdmin)