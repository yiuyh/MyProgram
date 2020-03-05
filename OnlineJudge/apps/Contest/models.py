from django.db import models
from Issue.models import Problem
from UserProfile.models import User
ATTRIBUTE_CHOICE = (('私有', '私有'), ('公开', '公开'))
LANGUAGE_CHOICE = (('C', 'C'), ('C++', 'C++'), ('Java', 'Java'), ('Python', 'Python'))


class Match(models.Model):                                                                        # 比赛列表
    matchName = models.CharField(max_length=30)                                          # 比赛名称
    startTime = models.DateTimeField()                                                                # 开始时间
    howLong = models.IntegerField(default=120, blank=True)                                            # 比赛时长
    attribute = models.CharField(max_length=2, choices=ATTRIBUTE_CHOICE, default='公开')              # 比赛属性
    include = models.ManyToManyField(to=Problem)                                                   # 比赛包含的题目
    status = models.IntegerField(default=0)                                                           # contest status
    info = models.TextField(blank=True,default='这个出题人很懒，没有比赛说明...')                           #比赛说明      
    registerNum = models.IntegerField(default=0)
    owner = models.ForeignKey(to=User,  on_delete=models.CASCADE, default='')
    def __str__(self):
        return self.matchName


class MatchSubmit(models.Model):                                                                         # 比赛提交
    match  = models.ForeignKey(to=Match, on_delete=models.CASCADE, default='')                                                                      # 比赛ID
    user = models.ForeignKey(to=User, on_delete=models.CASCADE, default='')                                               # 用户名
    problem = models.ForeignKey(to=Problem, on_delete=models.CASCADE, default='')                                                                       # 题目编号
    content = models.TextField(blank=True)                                                               # 提交代码
    runID = models.AutoField(primary_key=True)                                                          # 运行编号
    result = models.CharField(max_length=2, default='0', blank=True)                                    # 运行结果
    time = models.IntegerField(blank=True, default=0)                                                   # 运行时间
    memory = models.IntegerField(blank=True, default=0)                                                 # 运行内存
    language = models.CharField(default='C++', blank=True, max_length=9, choices=LANGUAGE_CHOICE)      # 语言
    subTime = models.DateTimeField(auto_now=True)                                                       # 提交时间

    def __str__(self):
        return str(self.match)


class MatchRank(models.Model):                                                                          # 比赛排名
    matchName = models.CharField(max_length=30, blank=True)                                             # 比赛名称
    userName = models.CharField(max_length=30, blank=True)                                              # 用户名
    acTime = models.IntegerField(default=0, blank=True)                                                 # AC时间
    wrongTimes = models.IntegerField(blank=True, default=0)                                             # 错误次数
    score = models.IntegerField(default=0, blank=True)                                                  # 比赛得分
    ranking = models.IntegerField(default=0, blank=True)                                                # 比赛排名


    def __str__(self):
        return self.score


class MatchRegister(models.Model):                                                                       # 注册比赛
    match = models.ForeignKey(to=Match, on_delete=models.CASCADE, default='')  # 比赛ID
    user = models.ForeignKey(to=User, on_delete=models.CASCADE, default='')  # 用户名
    registerTime = models.DateTimeField(auto_now=True)                                                  # 注册时间

    def __str__(self):
        return self.match
