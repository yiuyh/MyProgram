put files in manage.py dir



compile


g++ -o judge_client judge_client.cc `mysql_config --cflags --libs`
g++ -o judge_server judge_server.cpp `mysql_config --cflags --libs`



run

sudo ./judge_server
