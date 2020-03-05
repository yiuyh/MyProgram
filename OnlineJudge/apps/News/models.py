from django.db import models

# Create your models here.

class Info(models.Model):                                                                    
    title = models.TextField(default='无标题', blank=True)                                   
    content = models.TextField(default='这个人很懒，没有写任何内容...', blank=True)               
    subTime = models.DateTimeField(auto_now=True)                                                   # 提交时间

    def __str__(self):
        return self.title