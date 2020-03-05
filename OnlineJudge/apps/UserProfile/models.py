from django.db import models
from django.contrib.auth.models import AbstractUser
import os


def user_directory_path(instance, filename):
    ext = filename.split('.')[-1]
    filename = '{}.{}'.format(instance.username, ext)
    return os.path.join('media/user/', str(instance.id), filename)


class User(AbstractUser):
    GENDER_CHOICE = (('男', '男'), ('女', '女'))
    nickname = models.CharField(max_length=50, blank=True, default=True)
    school = models.CharField(max_length=30, default="贵州大学", blank=True)                              # 学校
    major = models.CharField(max_length=10, blank=True)                                                    # 专业
    myClass = models.CharField(max_length=15, blank=True)                                                  # 班级
    stuId = models.CharField(max_length=20, blank=True)                                                    # 学号
    headImage = models.ImageField(upload_to=user_directory_path, default="/static/image/default.jpg", blank=True)               # 头像
    synopsis = models.TextField(default="这个人很懒，什么都没有写...", blank=True)                        # 简介
    tel = models.CharField(max_length=11, blank=True)                                                     # 手机号
    rating = models.IntegerField(default=1500)                                                            # rating
    ac_nums = models.IntegerField(default=0)                                                              # accept nums

    class Meta(AbstractUser.Meta):
        pass
