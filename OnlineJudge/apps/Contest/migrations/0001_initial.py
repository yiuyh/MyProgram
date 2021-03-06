# -*- coding: utf-8 -*-
# Generated by Django 1.11.8 on 2020-01-28 17:56
from __future__ import unicode_literals

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Match',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('matchName', models.CharField(max_length=30)),
                ('startTime', models.DateTimeField()),
                ('howLong', models.IntegerField(blank=True, default=120)),
                ('attribute', models.CharField(choices=[('私有', '私有'), ('公开', '公开')], default='公开', max_length=2)),
                ('status', models.IntegerField(default=0)),
                ('info', models.TextField(blank=True, default='这个出题人很懒，没有比赛说明...')),
                ('registerNum', models.IntegerField(default=0)),
            ],
        ),
        migrations.CreateModel(
            name='MatchRank',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('matchName', models.CharField(blank=True, max_length=30)),
                ('userName', models.CharField(blank=True, max_length=30)),
                ('acTime', models.IntegerField(blank=True, default=0)),
                ('wrongTimes', models.IntegerField(blank=True, default=0)),
                ('score', models.IntegerField(blank=True, default=0)),
                ('ranking', models.IntegerField(blank=True, default=0)),
            ],
        ),
        migrations.CreateModel(
            name='MatchRegister',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('registerTime', models.DateTimeField(auto_now=True)),
            ],
        ),
        migrations.CreateModel(
            name='MatchSubmit',
            fields=[
                ('content', models.TextField(blank=True)),
                ('runID', models.AutoField(primary_key=True, serialize=False)),
                ('result', models.CharField(blank=True, default='0', max_length=2)),
                ('time', models.IntegerField(blank=True, default=0)),
                ('memory', models.IntegerField(blank=True, default=0)),
                ('language', models.CharField(blank=True, choices=[('C', 'C'), ('C++', 'C++'), ('Java', 'Java'), ('Python', 'Python')], default='C++', max_length=9)),
                ('subTime', models.DateTimeField(auto_now=True)),
                ('match', models.ForeignKey(default='', on_delete=django.db.models.deletion.CASCADE, to='Contest.Match')),
            ],
        ),
    ]
