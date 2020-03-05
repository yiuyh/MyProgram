from rest_framework import serializers

from UserProfile.models import User, UserToken


class UserSerializer(serializers.ModelSerializer):

    def create(self):
        data = self.data
        user = User.objects.create_user(
            username = data['username'],password=data['password'],stuId=data.get('stuId',""),
            school=data.get('school',""),email=data.get('email',""),tel=data.get('tel',""),)
        return UserSerializer(user), user

    class Meta:
        model = User
        fields = '__all__'
        # fields = ("id",  "username", "password", "GENDER_CHOICE", "school", "major", "myClass",
        #           "stuId", "headImage", "synopsis", "tel", "rating", "ac_nums")


class OtherUserSerializer(serializers.ModelSerializer):
    class Meta:
        model = User
        fields = ("nickname", "GENDER_CHOICE", "school", "major", "headImage", "synopsis", "rating", "ac_nums")


class UserListSerializer(serializers.ModelSerializer):
    class Meta:
        model = User
        fields = ("id", "username", "nickname", "myClass")



class UserTokenSerializer(serializers.ModelSerializer):
    class Meta:
        model = UserToken
        fields = '__all__'

        # depth = 1 #代表找嵌套关系的第几层
        # read_only_fields = ["id"] #将多个字段指定为只读


