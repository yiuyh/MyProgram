import xadmin
from Issue.models import *
class IssueAdmin():
    list_display = ['title', 'ratio', 'ac_nums', 'submit_nums']
    style_fields = {'content': 'ueditor'}
    menu_style = "accordion"

xadmin.site.register(Problem, IssueAdmin)