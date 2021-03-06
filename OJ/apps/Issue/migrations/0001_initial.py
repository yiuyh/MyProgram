# Generated by Django 2.2 on 2020-02-21 07:49

import Issue.models
from django.conf import settings
from django.db import migrations, models
import django.db.models.deletion
import tinymce.models


class Migration(migrations.Migration):

    initial = True

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='Problem',
            fields=[
                ('no', models.IntegerField(primary_key=True, serialize=False)),
                ('title', models.CharField(max_length=20, unique=True)),
                ('content', tinymce.models.HTMLField(blank=True, default='')),
                ('InputFormat', tinymce.models.HTMLField(blank=True, default='')),
                ('OutputFormat', tinymce.models.HTMLField(blank=True, default='')),
                ('time', models.IntegerField(blank=True, default=1000)),
                ('memory', models.IntegerField(blank=True, default=256)),
                ('tips', tinymce.models.HTMLField(blank=True, default='')),
                ('probSource', models.CharField(blank=True, max_length=10)),
                ('classification', models.CharField(blank=True, choices=[('搜索', '搜索'), ('计算几何', '计算几何'), ('数学', '数学'), ('数据结构', '数据结构'), ('动态规划', '动态规划'), ('图论', '图论'), ('基本算法', '基本算法')], max_length=10)),
                ('probAuthority', models.CharField(choices=[('公开', '公开'), ('隐藏', '隐藏')], default='公开', max_length=2)),
                ('weight', models.IntegerField(default=100)),
                ('ratio', models.FloatField(default=0.0)),
                ('ac_nums', models.IntegerField(default=0)),
                ('submit_nums', models.IntegerField(default=0)),
            ],
        ),
        migrations.CreateModel(
            name='test',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('test', models.CharField(max_length=20)),
            ],
        ),
        migrations.CreateModel(
            name='ProblemSubmit',
            fields=[
                ('content', models.TextField(blank=True)),
                ('runID', models.AutoField(primary_key=True, serialize=False)),
                ('result', models.CharField(blank=True, default='0', max_length=15)),
                ('time', models.IntegerField(blank=True, default=0)),
                ('memory', models.IntegerField(blank=True, default=0)),
                ('language', models.CharField(blank=True, choices=[('C', 'C'), ('C++', 'C++'), ('Java', 'Java'), ('Python', 'Python')], default='C++', max_length=9)),
                ('subTime', models.DateTimeField(auto_now=True)),
                ('problem', models.ForeignKey(default='', on_delete=django.db.models.deletion.CASCADE, to='Issue.Problem')),
                ('user', models.ForeignKey(default='', on_delete=django.db.models.deletion.CASCADE, to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.CreateModel(
            name='ProblemExample',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('sampleInput', tinymce.models.HTMLField(blank=True, default='')),
                ('sampleOutput', tinymce.models.HTMLField(blank=True, default='')),
                ('problem', models.ForeignKey(default='', on_delete=django.db.models.deletion.CASCADE, to='Issue.Problem')),
            ],
        ),
        migrations.CreateModel(
            name='ProblemData',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('standardInput', models.FileField(blank=True, upload_to=Issue.models.save_standard_input)),
                ('standardOutput', models.FileField(blank=True, upload_to=Issue.models.save_standard_output)),
                ('problem', models.ForeignKey(default='', on_delete=django.db.models.deletion.CASCADE, to='Issue.Problem')),
            ],
        ),
    ]
