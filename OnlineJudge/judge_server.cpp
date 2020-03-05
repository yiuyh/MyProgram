/*
Copyright 2018 Gzu_lx <250425650@qq.com>

评测机判题服务端模块、负责交互功能
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <mysql/mysql.h>

//C语言库函数
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
#include <signal.h>
#include <time.h>
#include <assert.h>
#include <ifaddrs.h>
#include <netdb.h>
#include <pthread.h>
#include <fcntl.h>
#include <time.h>
#include <dirent.h>
#include <stdarg.h>

//POSIX标准
#include <unistd.h>

//linux系统函数
#include <sys/socket.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <sys/time.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <sys/vfs.h>
#include <sys/select.h>
#include <sys/param.h>
#include <sys/stat.h>
#include <sys/resource.h>

#define DEBUG 1

#define OJ_WT 0 //等待判题
#define OJ_AC 1 //答案正确
#define OJ_PE 2 //格式错误
#define OJ_WA 3 //答案错误
#define OJ_TL 4 //时间超限
#define OJ_ML 5 //内存超限
#define OJ_OL 6 //输出超限
#define OJ_RE 7 //运行错误
#define OJ_CE 8 //编译错误
#define OJ_CO 9 //编译超限

#define STD_MB 1048576

static bool STOP = false; // 默认情况下OJ是运行的
static char oj_home[105] = {0};

// ========= mysql start ==============
MYSQL *g_conn; // mysql 连接
MYSQL_RES *g_res; // mysql 记录集
MYSQL_ROW g_row; // 字符串数组，mysql 记录行

#define MAX_BUF_SIZE 1024 // 缓冲区最大字节数

const char *g_host_name = "localhost";
const char *g_user_name = "root";
const char *g_password = "201314";
const char *g_db_name = "OnlineJudge";
const unsigned int g_db_port = 3306;
// ========= mysql end ================

void print_mysql_error(const char *msg) { // 打印最后一次错误
    if (msg)
        printf("%s: %s\n", msg, mysql_error(g_conn));
    else
        puts(mysql_error(g_conn));
}

int executesql(const char * sql) {
    /*query the database according the sql*/
    if (mysql_real_query(g_conn, sql, strlen(sql))) // 如果失败
        return -1; // 表示失败

    return 0; // 成功执行
}


int init_mysql() { // 初始化连接
    // init the database connection
    g_conn = mysql_init(NULL);
    /*设置字符编码,可能会乱码*/
    mysql_query(g_conn,"set nemas utf-8");
    /* connect the database */
    if(!mysql_real_connect(g_conn, g_host_name, g_user_name, g_password, g_db_name, g_db_port, NULL, 0)) // 如果失败
        return -1;

    // 是否连接已经可用
    //if (executesql("set names utf8")) // 如果失败
       // return -1;
    return 0; // 返回成功
}



// 用RabbitMQ和web端进行通信
void MessageQueue_Web() {


}




//当有代评测提交，并且进程数允许的情况下，创建新的子进程调用该评测函数
//输入：代评测提交的solution_id, 子进程在ID[]中的保存位置 i
void run_client(char probID[], char runID[], char content[], char language[], int flag) {
    //在Linux系统中，Resouce limit指在一个进程的执行过程中，它所能得到的资源的限制，
    //比如进程的core file的最大值，虚拟内存的最大值等 ，这是运行时间，内存大小实现的关键
    /*
    结构体中 rlim_cur是要取得或设置的资源软限制的值，rlim_max是硬限制
    这两个值的设置有一个小的约束：
    1） 任何进程可以将软限制改为小于或等于硬限制
    2）任何进程都可以将硬限制降低，但普通用户降低了就无法提高，该值必须等于或大于软限制
    3） 只有超级用户可以提高硬限制

    setrlimit(int resource,const struct rlimit rlptr);返回：若成功为0，出错为非0
    RLIMIT_CPU：CPU时间的最大量值（秒），当超过此软限制时向该进程发送SIGXCPU信号
    RLIMIT_FSIZE:可以创建的文件的最大字节长度，当超过此软限制时向进程发送SIGXFSZ
    */
    struct rlimit LIM;
    LIM.rlim_max = 800;
    LIM.rlim_cur = 800;
    setrlimit(RLIMIT_CPU, &LIM);//cpu运行时间限制

    LIM.rlim_max = 80 * STD_MB;
    LIM.rlim_cur = 80 * STD_MB;
    setrlimit(RLIMIT_FSIZE, &LIM);//可文件大小限制，防止恶意程序的吗？

    LIM.rlim_max = STD_MB << 11;//左移11 STD_MB是2^20 MB 2^11MB 2GB机器起码的2GB虚拟内存？
    LIM.rlim_cur = STD_MB << 11;
    setrlimit(RLIMIT_AS, &LIM);//最大运行的虚拟内存大小限制

    LIM.rlim_cur = LIM.rlim_max = 200;
    setrlimit(RLIMIT_NPROC, &LIM);//每个实际用户ID所拥有的最大子进程数，这些都是为了防止恶意程序的吧？？


    //write_log("sid=%s\tclient=%s\toj_home=%s\n",runidstr,buf,oj_home);
    //sprintf(err,"%s/run%d/error.out",oj_home,clientid);
    //freopen(err,"a+",stderr);
    printf("execl..........................................................\n");
    if(flag == 1)
        execl("./judge_client", "./judge_client",probID, runID, content, language, "1", (char *) NULL);
    else 
        execl("./judge_client", "./judge_client",probID, runID, content, language, "0", (char *) NULL);
    //exit(0);
}


struct problem {
    char source[40960];
    char problem_id[10];
    char run_id[10];
    char language[10];
    int flag;//0 Issue_problemsubmit 1 Contest_matchsubmit
}Problem[105];

int find_result_mysql() {
    if (init_mysql());
        print_mysql_error(NULL);
    int cnt = 0;
    char sql[MAX_BUF_SIZE];

    if (executesql("select probID,runID,content,language from Contest_matchsubmit where result = 0 limit 30")) // 句末没有分号
        print_mysql_error(NULL);

    g_res = mysql_store_result(g_conn); // 从服务器传送结果集至本地，mysql_use_result直接使用服务器上的记录集

    int iNum_rows = mysql_num_rows(g_res); // 得到记录的行数
    int iNum_fields = mysql_num_fields(g_res); // 得到记录的列数

    printf("contest  共%d个记录，每个记录%d字段\n", iNum_rows, iNum_fields);

    puts("probID\tcontent\trunID\n");
    while ((g_row=mysql_fetch_row(g_res))) {// 打印结果集
        //printf("%s\t%s\t%s\n\n\n", g_row[2], g_row[3], g_row[4]); // 第一，第二字段
        strcpy(Problem[cnt].problem_id, g_row[0]);
        strcpy(Problem[cnt].run_id, g_row[1]);
        strcpy(Problem[cnt].source, g_row[2]);
        strcpy(Problem[cnt].language, g_row[3]);
        Problem[cnt].flag = 1;
        cnt++;
    }
    mysql_free_result(g_res); // 释放结果集

    
    if (executesql("select probID,runID,content,language from Issue_problemsubmit where result = 0 limit 30")) // 句末没有分号
        print_mysql_error(NULL);

    g_res = mysql_store_result(g_conn); // 从服务器传送结果集至本地，mysql_use_result直接使用服务器上的记录集

    int pro_iNum_rows = mysql_num_rows(g_res); // 得到记录的行数
    int pro_iNum_fields = mysql_num_fields(g_res); // 得到记录的列数

    printf("problem  共%d个记录，每个记录%d字段\n", pro_iNum_rows, pro_iNum_fields);

    puts("probID\tcontent\trunID\n");
    while ((g_row=mysql_fetch_row(g_res))) {// 打印结果集
        //printf("%s\t%s\t%s\n\n\n", g_row[2], g_row[3], g_row[4]); // 第一，第二字段
        strcpy(Problem[cnt].problem_id, g_row[0]);
        strcpy(Problem[cnt].run_id, g_row[1]);
        strcpy(Problem[cnt].source, g_row[2]);
        strcpy(Problem[cnt].language, g_row[3]);
        Problem[cnt].flag = 0;
        cnt++;
    }
    mysql_free_result(g_res); // 释放结果集
    

    mysql_close(g_conn); // 关闭链接

    return iNum_rows + pro_iNum_rows;
}


int ID[105] = {0};
int SID[105] = {0};

int solve() {
    // find result in mysql
    int ret_num = find_result_mysql();
    if (ret_num == 0) return 0;

    static int max_running = 8;
    static int retcnt = 0;//成功测评的次数
    static int workcnt = 0;//目前同时在测评的client的数量
    //从消息队列里接收web端传来的消息  源代码+题目id

    for (int i = 0; i < max_running; i++) {
        if (ID[i] != 0) workcnt++;
    }
    int w = 0;
    int tmp_pid = 0;
    //fork多个子进程进行分布式测评
    for (int i = 0; i < max_running; i++) {
        if (w >= ret_num) break;
        if (workcnt >= max_running) {           // if no more client can running
            //如果达到了可用最大进程数目，那么等待一个子进程结束
            //waitpid，参考linux 下 c 语言编程下的 进程管理
            //waitpid()会暂时停止目前进程的执行，直到有信号来到或子进程结束
            //pid_t waitpid(pid_t pid,int * status,int options);
            //pid=-1 代表任意子进程；status 取回子进程识别码，这里不需要所以NULL;
            //参数options提供了一些额外的选项来控制waitpid，比如不等待继续执行，这里0代表不使用，进程挂起
            //如果 有子进程已经结束，那么执行到这里的时候会直接跳过，子进程也会由僵尸进程释放
            //返回结束的子进程pid
            tmp_pid = waitpid(-1, NULL, 0);     // wait 4 one child exit
            workcnt--;//子进程结束了个，那么现用judge_client数量减一
            retcnt++;//评测完成数加1
            //清除保存在 ID[]里的已经结束的子进程信息
            for (i = 0; i < max_running; i++)     // get the client id
                if (ID[i] == tmp_pid)
                    break; // got the client id
            ID[i] = 0;
        } else {                                             // have free client
            for (i = 0; i < max_running; i++)     // find the client id
                if (ID[i] == 0)
                    break;    // got the client id
        }
        if (workcnt < max_running) {
            workcnt++;
            ID[i] = fork();
            if (ID[i] == 0) {
                printf("running %s\t%s\t%s\t%s\t%d\n\n\n", Problem[w].problem_id, Problem[w].run_id, Problem[w].source, Problem[w].language, Problem[w].flag);
                run_client(Problem[w].problem_id, Problem[w].run_id, Problem[w].source, Problem[w].language, Problem[w].flag);  //在子进程里更新ID[0]=pid  // if the process is the son, run it
                exit(0);//子进程执行完毕退出0，父进程不会执行这段if ，在run_client里进程会跳转到execl(judge_client)
            }
            w++;
        }
        else {
            ID[i] = 0;
        }
    }


    while ((tmp_pid = waitpid(-1, NULL, WNOHANG)) > 0) {
        workcnt--;
        retcnt++;
        int i = 0;
        for (i = 0; i < max_running; i++)     // get the client id
            if (ID[i] == tmp_pid)
                break; // got the client id
        ID[i] = 0;
        printf("tmp_pid = %d\n", tmp_pid);
    }

    //处理意外产生的僵尸进程


    //返回已经评测的次数
    return retcnt;
}



int daemon_init() {
	pid_t pid;
	if ((pid = fork()) < 0) return -1;
	else if (pid != 0)
		exit(0); /* parent exit */

	/* child continues */
	setsid(); /* become session leader */
	chdir(oj_home); /* change working directory */
	umask(0); /* clear file mode creation mask */
	close(0); /* close stdin */
	close(1); /* close stdout */
	close(2); /* close stderr */

	int fd = open( "/dev/null", O_RDWR );
	dup2(fd, 0);
	dup2(fd, 1);
	dup2(fd, 2);
	if (fd > 2) {
		close(fd);
	}
	return 0;
}

// 
void call_for_exit(int s) {
    STOP = true;
    printf("Stopping judged...\n");
}

int main(int argc, char** argv) {
    //strcpy(oj_home, "/usr/local/judge");
    //chdir(oj_home); // change the dir

    if (!DEBUG) //如果不是调试状态则启动守护进程
        daemon_init(); // 创建一个守护进程

    //对一些中断信号进行处理防止程序意外挂掉
    signal(SIGQUIT, call_for_exit);
    signal(SIGKILL, call_for_exit);
    signal(SIGTERM, call_for_exit);

    // 循环等待与处理web端传来的判题消息
    int flag = 1;
    while (true) {
        while (flag) { // 优先处理web端传来的判题消息
            flag = solve();
        }
        printf("sleep......\n");
        sleep(1);
        flag = 1;
    }
    return 0;
}
