from rest_framework import serializers

from .models import *


class ProblemSerializer(serializers.ModelSerializer):

    class Meta:
        model = Problem
        fields = '__all__'


class ProblemListSerializer(serializers.ModelSerializer):

    class Meta:
        model = Problem
        # fields = '__all__'
        fields = ("no", "title", "classification", "probAuthority")


class ProblemDataSerializer(serializers.ModelSerializer):

    class Meta:
        model = ProblemData
        fields = ("standardInput","standardOutput")


class ProblemExampleSerializer(serializers.ModelSerializer):

    class Meta:
        model = ProblemExample
        fields = ("sampleInput","sampleOutput")


class ProblemSubmitListSerializer(serializers.ModelSerializer):
    user_Id = serializers.IntegerField(source="user.id")
    user_Username = serializers.CharField(source="user.username")
    problem_No = serializers.IntegerField(source="problem.no")
    problem_Title = serializers.CharField(source="problem.title")

    class Meta:
        model = ProblemSubmit
        fields = ("runID", "result", "time", "memory", "language", "subTime", "user_Id", "user_Username", "problem_No", "problem_Title")

class ProblemSubmitSerializer(serializers.ModelSerializer):
    user_Id = serializers.IntegerField(source="user.id")
    user_Username = serializers.CharField(source="user.username")
    problem_No = serializers.IntegerField(source="problem.no")
    problem_Title = serializers.CharField(source="problem.title")

    class Meta:
        model = ProblemSubmit
        fields = '__all__'


# class TestSerializer(serializers.ModelSerializer):
#     class Meta:
#         model = test
#         fields = '__all__'

