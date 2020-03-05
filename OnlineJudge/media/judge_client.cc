/*
Copyright 2018 Gzu_lx <250425650@qq.com>

评测机判题模块、负责判题功能


支持语言：C/C++/Java/Python
*/

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
#include <ctype.h>

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
#include <sys/ptrace.h>
#include <sys/user.h>
#include <sys/reg.h>
#include <sys/syscall.h>
#include <sys/signal.h>

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <mysql/mysql.h>

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

static int submit_flag = 0;

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
#define STD_T_LIM 2
#define STD_F_LIM (STD_MB<<5)
#define STD_M_LIM (STD_MB<<7)
#define BUFFER_SIZE 512
#define ZOJ_COM 0// zoj compare

#ifdef __i386
#define REG_SYSCALL orig_eax
#define REG_RET eax
#define REG_ARG0 ebx
#define REG_ARG1 ecx
//c & c++
int LANG_CV[256] = { 3,4,8,11,33,45,85,122,140,146,192,197,243,252,257,272,SYS_time, SYS_read, SYS_uname, SYS_write, SYS_open,
		SYS_close, SYS_execve, SYS_access, SYS_brk, SYS_munmap, SYS_mprotect,
		SYS_mmap2, SYS_fstat64, SYS_set_thread_area, 252, 0 };
#else
#define REG_SYSCALL orig_rax
#define REG_RET rax
#define REG_ARG0 rdi
#define REG_ARG1 rsi
//c & c++
int LANG_CV[256] = {0,1,2,3,4,5,8,9,10,11,12,20,21,59,63,89,158,231,240,257,272,511, SYS_time, SYS_read, SYS_uname, SYS_write, SYS_open,
		SYS_close, SYS_execve, SYS_access, SYS_brk, SYS_munmap, SYS_mprotect,
		SYS_mmap, SYS_fstat, SYS_set_thread_area, 252, SYS_arch_prctl, 0 };
#endif

static const int DEBUG = 1;


//system升级版、避免系统信号突然改变的坑
int execute_cmd(char *cmd)
{
	FILE *pp = popen(cmd, "r"); //建立管道
	if (pp == NULL) {
        char log[1024] = {0};
		sprintf(log, "popen函数执行出错:%s", strerror(errno));
		printf("%s\n", log);
		return -1;
    }
	char tmp[1024] = {0};
	int len = 0;
	int len_tmp = 0;
    while(fgets(tmp, sizeof(tmp), pp) != NULL)
	{
		len_tmp = strlen(tmp);
		len = len + len_tmp;
		if(len > 3500)
		{
			printf("运行结果输出太长，无法展示完\n");
			break;
		}
	}
	
	int res = 0;
    if((res = pclose(pp)) == -1)//关闭管道
	{
		printf("popen关闭函数出错!\n");
		return -1;
	}
	return res;
}

/*
int execute_cmd(char cmd[]) {
	int ret = 0;
	if (DEBUG) printf("cmd:%s\n", cmd);
	ret = system(cmd);
	return ret;
}
*/

long get_file_size(const char * filename) {
	struct stat f_stat;

	if (stat(filename, &f_stat) == -1) {
		return 0;
	}

	return (long) f_stat.st_size;
}


const int call_array_size = 512;
unsigned int call_counter[call_array_size] = { 0 };
void init_syscalls_limits() {
	for (int i = 0; i < call_array_size; i++) {
		call_counter[i] = 0;
	}
	for (int i = 0; i == 0 || LANG_CV[i]; i++) {
		call_counter[LANG_CV[i]] = 1;
	}
}

struct problem_limit {
	char time[20];
	char memory[20];
}Pro_limit;

int find_result_mysql(char* problem_id) {
    if (init_mysql());
        print_mysql_error(NULL);

    char sql[MAX_BUF_SIZE] = {0};
    sprintf(sql, "select time,memory from Issue_problem where no = %s", problem_id);
    if (executesql(sql)) // 句末没有分号
        print_mysql_error(NULL);

    g_res = mysql_store_result(g_conn); // 从服务器传送结果集至本地，mysql_use_result直接使用服务器上的记录集

    int iNum_rows = mysql_num_rows(g_res); // 得到记录的行数
    int iNum_fields = mysql_num_fields(g_res); // 得到记录的列数

    printf("共%d个记录，每个记录%d字段\n", iNum_rows, iNum_fields);

    //puts("time\tmemory\n");
    int cnt = 0;
    while ((g_row=mysql_fetch_row(g_res))) {// 打印结果集
        //printf("%s\t%s\t%s\n\n\n", g_row[2], g_row[3], g_row[4]); // 第一，第二字段
        strcpy(Pro_limit.time, g_row[0]);
        strcpy(Pro_limit.memory, g_row[1]);
    }
    mysql_free_result(g_res); // 释放结果集

    mysql_close(g_conn); // 关闭链接

    return iNum_rows;
}

void update_result_mysql(char* runID, int ACflag, int usedtime, int topmemory) {
	if (init_mysql());
        print_mysql_error(NULL);

    char sql[MAX_BUF_SIZE] = {0};
    printf("submitflag %d .....................................\n", submit_flag);
    if (submit_flag)
    	sprintf(sql, "UPDATE Contest_matchsubmit SET result=%d,time=%d,memory=%d WHERE runID=%s", ACflag, usedtime, topmemory,runID);
    else
    	sprintf(sql, "UPDATE Issue_problemsubmit SET result=%d,time=%d,memory=%d WHERE runID=%s", ACflag, usedtime, topmemory,runID);
    if (executesql(sql)) // 句末没有分号
        print_mysql_error(NULL);

    g_res = mysql_store_result(g_conn); // 从服务器传送结果集至本地，mysql_use_result直接使用服务器上的记录集

    mysql_free_result(g_res); // 释放结果集

    mysql_close(g_conn); // 关闭链接
}



int compile(int lang, char* submit_id) {
	char compile_str[105] = {0};
	if (lang == 1) {//c
		sprintf(compile_str, "g++ %s.c -o %s", submit_id, submit_id);
	}
	else if (lang == 2) {//c++
		sprintf(compile_str, "g++ %s.cpp -o %s", submit_id, submit_id);
	}
	else {//python  python -m py_compile $filename
		//sprintf(compile_str, "python -m py_compile %s.py", submit_id);
		return 0;
	}
	printf("compile_str:%s\n", compile_str);
	pid_t pid = fork();
	if (pid < 0) {
		printf("fork error......");
	}
	if (pid == 0) { // child pid
		printf("client child..................\n");
		//resource limit
		struct rlimit LIM;
		LIM.rlim_max = 60;
		LIM.rlim_cur = 60;
		setrlimit(RLIMIT_CPU, &LIM);
		alarm(30);//max 60s kill pid
		LIM.rlim_max = 100 * STD_MB;
		LIM.rlim_cur = 100 * STD_MB;
		setrlimit(RLIMIT_FSIZE, &LIM);
		LIM.rlim_max = STD_MB << 10;
		LIM.rlim_cur = STD_MB << 10;
		setrlimit(RLIMIT_AS, &LIM);

		//output error message
		char ce_error[4096] = {0};
		//sprintf(ce_error, "%s_error.txt", submit_id);
		//freopen(ce_error, "w", stderr);

		//safe
		//execute_cmd("chown verticallimit *");
		//while(setgid(1536)!=0) sleep(1);
        //while(setuid(1536)!=0) sleep(1);
        //while(setresuid(1536, 1536, 1536)!=0) sleep(1);

        int ans = execute_cmd(compile_str);
        printf("ans %d\n", ans);
        if (ans == -1) exit(1);
    	else exit(0);
	}
	else { // father pid
		int status = 0;
		waitpid(pid, &status, 0);
		if (DEBUG) printf("status = %d\n", status);
		return status;
	}
}


int run_solution(int lang, int time_lmt, int usedtime, int mem_lmt, char* input_id, char* submit_id) {
	//printf("run_solution 233\n");
	char input_path[105] = {0};
	sprintf(input_path, "./%s", input_id);
	freopen(input_path, "r", stdin);
	char output_path[105] = {0};
	sprintf(output_path, "%s_out.out", submit_id);
	freopen(output_path, "w", stdout);
	char error_path[105] = {0};
	sprintf(error_path, "%s_error.txt", submit_id);
	freopen(error_path, "a+", stderr);
	
	ptrace(PTRACE_TRACEME, 0, NULL, NULL);
	
	struct rlimit LIM;
	
	// time limit
	LIM.rlim_cur = (time_lmt - usedtime / 1000) + 1;
	LIM.rlim_max = LIM.rlim_cur;
	setrlimit(RLIMIT_CPU, &LIM);
	alarm(0);
	
	// file limit
	LIM.rlim_max = STD_F_LIM + STD_MB;
	LIM.rlim_cur = STD_F_LIM;
	setrlimit(RLIMIT_FSIZE, &LIM);
	// proc limit
	LIM.rlim_cur = LIM.rlim_max = 1;
	setrlimit(RLIMIT_NPROC, &LIM);
	// set the stack
	LIM.rlim_cur = STD_MB << 6;
	LIM.rlim_max = STD_MB << 6;
	setrlimit(RLIMIT_STACK, &LIM);
	
	// set the memory
	LIM.rlim_cur = STD_MB * mem_lmt * 2;
	LIM.rlim_max = STD_MB * mem_lmt * 2;
	setrlimit(RLIMIT_AS, &LIM);
	
	// running
	//printf("mmmmmm\n");
	//int res = 0;
	char func_name[40] = {0};
	//if (lang == 1 || lang == 2) {
		sprintf(func_name, "./%s", submit_id);
		execl(func_name, func_name, (char *) NULL);
	//}
	//else {
		//sprintf(func_name, "./%s.py", submit_id);	
		//execl("python", "python", func_name, (char *) NULL);
	//}
	//res = execl("/bin/ls","ls",(char * )0);
	char pr_str[400] = {0};
	sprintf(pr_str, "execl ./%s", submit_id);
	perror(pr_str);
	//printf("res = %d\n", res);
	fflush(stderr);
	exit(0);
}

int get_proc_status(int pid, const char * mark) {
	FILE * pf;
	char fn[BUFFER_SIZE], buf[BUFFER_SIZE];
	int ret = 0;
	sprintf(fn, "/proc/%d/status", pid);
	pf = fopen(fn, "re");
	int m = strlen(mark);
	while (pf && fgets(buf, BUFFER_SIZE - 1, pf)) {

		buf[strlen(buf) - 1] = 0;
		if (strncmp(buf, mark, m) == 0) {
			sscanf(buf + m + 1, "%d", &ret);
		}
	}
	if (pf)
		fclose(pf);
	return ret;
}

void watch_solution(pid_t pidApp, char* userfile, char* outfile, \
	int& usedtime, int& topmemory, int time_lmt, \
	int mem_lmt, int& ACflag, char* submit_id) {
	// parent
	int tempmemory;
	int status, sig, exitcode;
	//与ptrace 相互配合，来进行跟踪调试执行子进程 
	struct user_regs_struct reg;
	struct rusage ruse;
	while (1) {
		//if (DEBUG) printf("watch_solution\n");
		wait4(pidApp, &status, 0, &ruse);
		
		tempmemory = get_proc_status(pidApp, "VmPeak:") << 10;
		//printf("memory %d\n", tempmemory);
		if (tempmemory > topmemory)
			topmemory = tempmemory;
		//内存超了就退出，并修改ACflag 
		if (topmemory > mem_lmt * STD_MB) {
			if (DEBUG)
				printf("out of memory %d\n", topmemory);
			if (ACflag == OJ_AC)
				ACflag = OJ_ML;
			ptrace(PTRACE_KILL, pidApp, NULL, NULL);//杀死子进程，停止执行 
			break;
		}

		if (WIFEXITED(status)) break; // child eixt error

		char error_path[105] = {0};
		sprintf(error_path, "%s_error.txt", submit_id);
		if (get_file_size(error_path)) {
			ACflag = OJ_RE;
			//printf("oj_re  222222\n");
			ptrace(PTRACE_KILL, pidApp, NULL, NULL);
			break;
		}
		if (get_file_size(userfile) > get_file_size(outfile) * 2 + 1024) {
			printf("userfile %ld outfile %ld\n", get_file_size(userfile), get_file_size(outfile));
			ACflag = OJ_OL;
			ptrace(PTRACE_KILL, pidApp, NULL, NULL);
			break;
		}

		if (WIFSIGNALED(status)) {
			
			/*  WIFSIGNALED: if the process is terminated by signal
			 * sig = 5 means Trace/breakpoint trap
			 * sig = 11 means Segmentation fault
			 * sig = 25 means File size limit exceeded
			 */
			 
			sig = WTERMSIG(status);
 
			if (DEBUG) {
				printf("WTERMSIG=%d\n", sig);
				psignal(sig, NULL);
			}
			if (ACflag == OJ_AC) {
				switch (sig) {
				case SIGCHLD:
				case SIGALRM:
					alarm(0);
				case SIGKILL:
				case SIGXCPU:
					ACflag = OJ_TL;
					break;
				case SIGXFSZ:
					ACflag = OJ_OL;
					printf("sigxfsz.....\n");
					break;
 
				default:
					ACflag = OJ_RE;
					//printf("defalg oj_re\n");
				}
				if (DEBUG) printf("ACflag:%d\n", ACflag);
			}
			break;
		}
		// check the system calls
		ptrace(PTRACE_GETREGS, pidApp, NULL, &reg);
		//unsigned int call_id = reg.REG_SYSCALL;
		if (call_counter[reg.REG_SYSCALL] == 1) {//white syscall list
			//continue;
		}
		else {
			ACflag = OJ_RE;
			//printf("oj_re xxxxx1111\n");
			char error[BUFFER_SIZE];
			printf("syscall not allow call:%ld\n",(long)reg.REG_SYSCALL);
			ptrace(PTRACE_KILL, pidApp, NULL, NULL);
		}
		
		//if (WIFEXITED(status)) break;
		ptrace(PTRACE_SYSCALL, pidApp, NULL, NULL);//子进程运行到下一次执行系统调用的时候
		// time limit 10s
		//if (time > )
	}
	usedtime += (ruse.ru_utime.tv_sec * 1000 + ruse.ru_utime.tv_usec / 1000);
	usedtime += (ruse.ru_stime.tv_sec * 1000 + ruse.ru_stime.tv_usec / 1000);
}


void delnextline(char s[]) {
	int L;
	L = strlen(s);
	while (L > 0 && (s[L - 1] == '\n' || s[L - 1] == '\r'))
		s[--L] = 0;
}

int compare(const char *file1, const char *file2) {
	//the original compare from the first version of hustoj has file size limit
	//and waste memory
	FILE *f1,*f2;
	char *s1,*s2,*p1,*p2;
	int PEflg;
	s1=new char[STD_F_LIM+512];
	s2=new char[STD_F_LIM+512];
	if (!(f1=fopen(file1,"r")))
	return OJ_AC;
	for (p1=s1;EOF!=fscanf(f1,"%s",p1);)
	while (*p1) p1++;
	fclose(f1);
	if (!(f2=fopen(file2,"r"))) {
	printf("compare oj_re\n");
	return OJ_RE;
	}
	for (p2=s2;EOF!=fscanf(f2,"%s",p2);)
	while (*p2) p2++;
	fclose(f2);
	if (strcmp(s1,s2)!=0) {
		//              printf("A:%s\nB:%s\n",s1,s2);
		delete[] s1;
		delete[] s2;
 
		return OJ_WA;
	} else {
		f1=fopen(file1,"r");
		f2=fopen(file2,"r");
		PEflg=0;
		while (PEflg==0 && fgets(s1,STD_F_LIM,f1) && fgets(s2,STD_F_LIM,f2)) {
			delnextline(s1);
			delnextline(s2);
			if (strcmp(s1,s2)==0) continue;
			else PEflg=1;
		}
		delete [] s1;
		delete [] s2;
		fclose(f1);fclose(f2);
		if (PEflg) return OJ_PE;
		else return OJ_AC;
	}
}


void judge_solution(int& ACflag, int& usedtime, int time_lmt, \
		int mem_lmt, int& topmemory, char* userfile, char* outfile) {
	int comp_res;
	if (ACflag == OJ_AC && usedtime > time_lmt * 1000) {
		ACflag = OJ_TL;
	}
	if (topmemory > mem_lmt * STD_MB) ACflag = OJ_ML;
	//compare
	if (ACflag == OJ_AC) {
		comp_res = compare(outfile, userfile);
		if (comp_res == OJ_WA) {
			ACflag = OJ_WA;
			if (DEBUG) printf("compare OJ_WA\n");
		}
		else if (comp_res == OJ_PE) {
			ACflag = OJ_PE;
			if (DEBUG) printf("compare OJ_PE\n");
		}
		ACflag = comp_res;
	}
}

void write_source(char* buf, int lang, char* submit_id) {
	char str[105] = {0};
	if (lang == 1) {
		sprintf(str, "%s.c", submit_id);
	}
	else if (lang == 2){
		sprintf(str, "%s.cpp", submit_id);
	}
	else {
		//sprintf(str, "%s.py", submit_id);
		sprintf(str, "%s.cpp", submit_id);
	}
	int fd = open(str, O_CREAT | O_TRUNC | O_RDWR, 0600);
	int len = strlen(buf);
	write(fd, buf, len);
	close(fd);
}

int Check_Compile(char* runID, int lang) {
	if (lang == 3) return 0;
	dirent * dirt;		// linux 下的结构体
    DIR * dir;;
    char *s[1000];
    int i=0,n=1;
    s[0]= new  char[100];
    memset(s[0],0,100);
    sprintf(s[0],"./");	
    printf("ffffffffffffffffff...........................\n");
    
    do{
        dir = opendir(s[i]);		// 打开指定路径
        if(dir!=NULL)
            while((dirt=readdir(dir))!=NULL){     // 存储路径下的所有文件和文件夹路径
                if(strcmp(dirt->d_name,".")==0||strcmp(dirt->d_name,"..")==0)
                    continue;
                s[n]= new  char[100];
                memset(s[n],0,100);
                sprintf(s[n],"%s/%s",s[i],dirt->d_name);
                n++;
            }
        i++;
    }while(i<n);
    
    printf("kkkkkkkkkkkkkk...........................\n");
    closedir(dir);
    int max_time = 0;
	int max_memory = 0;
	char temp_str[105] = {0};
	if (lang == 1)
		sprintf(temp_str, ".//%s", runID);
	else if (lang == 2)
		sprintf(temp_str, ".//%s", runID);
	else//java
		//sprintf(temp_str, ".//%s.pyc", runID);
		sprintf(temp_str, ".//%s", runID);
	int ok = 1;
    for (i = 0; i < n; i++) {     // 广度优先遍历文件夹
        printf("%s temp_str %s\n",s[i], temp_str);
        if (strcmp(s[i], temp_str) == 0) ok = 0;
    }
    return ok;
}


//probID runID content language
int main(int argc, char** argv) {
	//printf("see you again!\n\n\n");return 0;
	/*
	if (argc != 5) {
		if (DEBUG) printf("argc num not 5\n");
		return 1;
	}
	*/
	printf("client..............................");
	printf("%s\n", argv[1]);
	printf("%s\n", argv[2]);

	printf("%s\n", argv[3]);

	printf("%s\n", argv[4]);


	char probID[10] = {0};
	char runID[10] = {0};
	char content[40960] = {0};

	strcpy(probID, argv[1]);
	strcpy(runID, argv[2]);
	strcpy(content, argv[3]);
	if (strcmp(argv[5],"1") == 0) submit_flag = 1;//problem or contest


	int ans_num = find_result_mysql(probID);
	int time_lmt = 1;//unit s
	int mem_lmt = 256;
	int lang = 1;

	if (strcmp(argv[4], "C") == 0) {
		lang = 1;
	}
	else if (strcmp(argv[4], "C++") == 0) {
		lang = 2;
	}
	else {//
		lang = 2;
	}
	// find problem limit
	
	if (ans_num != 0) {
		time_lmt = atoi(Pro_limit.time)/1000;
		mem_lmt = atoi(Pro_limit.memory);
	}
	printf("time_lmt %d mem_lmt %d  .........\n", time_lmt, mem_lmt);
	

	int usedtime = 0;
	int ACflag = OJ_AC;

	//wirte source.cpp
	write_source(content, lang, runID);

	//compile source
	int Compile_OK = compile(lang, runID);
	printf("Check_Compile...........................\n");
	Compile_OK = Check_Compile(runID, lang);
	printf("Check_Compile %d............xxxxxxxxxxx......\n",Compile_OK);
	if (Compile_OK) { // compile failed
		if (DEBUG) printf("compile faile\n");
		ACflag = OJ_CE;

		//delete files
		char cmd[105] = {0};
		if (lang == 1)
			sprintf(cmd, "rm -rf %s.c", runID);
		else if (lang == 2)
			sprintf(cmd, "rm -rf %s.cpp", runID);
		//else
			//sprintf(cmd, "rm -rf %s.py", runID);
		int ret = execute_cmd(cmd);
		// update result
		update_result_mysql(runID, ACflag, 0, 0);
		exit(ret);
	}
	

	// find input files
	dirent * dirt;		// linux 下的结构体
    DIR * dir;;
    char *s[200];
    int i=0,n=1;
    s[0]= new  char[200];
    memset(s[0],0,200);
    sprintf(s[0],"./problems/%s", probID);	
    do{
        dir = opendir(s[i]);		// 打开指定路径
        if(dir!=NULL)
            while((dirt=readdir(dir))!=NULL){     // 存储路径下的所有文件和文件夹路径
                if(strcmp(dirt->d_name,".")==0||strcmp(dirt->d_name,"..")==0)
                    continue;
                s[n]= new  char[200];
                memset(s[n],0,200);
                sprintf(s[n],"%s/%s",s[i],dirt->d_name);
		printf("sn... %s\n", s[n]);
                n++;
            }
        i++;
    }while(i<n);
    closedir(dir);
    int max_time = 0;
	int max_memory = 0;
    for (i = 0; i < n; i++) {     // 广度优先遍历文件夹
        printf("find ...  %s\n",s[i]);
        int len_s = strlen(s[i]);
        if (s[i][len_s - 1] != 'n' || s[i][len_s - 2] != 'i' || s[i][len_s - 3] != '.') continue;
		//run_solution
		init_syscalls_limits();
		
		pid_t pidApp = fork();
		if (pidApp == 0) {//child pid
			//执行程序，在/run0/下生成user.out结果文件 
			printf("running... %s\n", s[i]);
			run_solution(lang, time_lmt, usedtime, mem_lmt, s[i], runID);
			exit(0);
		}
		else { // father pid
			
			int usedtime = 0;
			int topmemory = 0;

			char userfile[BUFFER_SIZE] = {0};

			for (int j = 0; j < strlen(s[i]); j++) userfile[j] = s[i][j];
			userfile[len_s - 1] = 'u';
			userfile[len_s - 2] = 'o';
			userfile[len_s] = 't';
			char outfile[BUFFER_SIZE] = {0};
			sprintf(outfile, "%s_out.out", runID);
			printf("compare %s %s\n", userfile, outfile);
			watch_solution(pidApp, userfile, outfile, usedtime, topmemory, time_lmt, mem_lmt, ACflag, runID);
			
			judge_solution(ACflag, usedtime, time_lmt, mem_lmt, topmemory, userfile, outfile);
			topmemory /= 1024;
			if (DEBUG) {
				printf("ACflag is:%d\n", ACflag);
				printf("usedtime:%d topmemory:%d\n", usedtime, topmemory);
				if (ACflag == 1) {
					printf("Accept!!\n");
				}
			}
			if (usedtime > max_time) max_time = usedtime;
			if (topmemory > max_memory) max_memory = topmemory;
			
			// update result
			if (ACflag != OJ_AC) {
				// delete files
				char cmd[105] = {0};
				sprintf(cmd, "rm -rf %s_out.out", runID);
				execute_cmd(cmd);

				memset(cmd, 0, sizeof(cmd));
				sprintf(cmd, "rm -rf %s_error.txt", runID);
				execute_cmd(cmd);
				
				// 删除生成的可执行文件
				memset(cmd, 0, sizeof(cmd));
				sprintf(cmd, "rm -rf %s", runID);
				execute_cmd(cmd);

				memset(cmd, 0, sizeof(cmd));
				if (lang == 1)
					sprintf(cmd, "rm -rf %s.c", runID);
				else if (lang == 2)
					sprintf(cmd, "rm -rf %s.cpp", runID);
				else
					sprintf(cmd, "rm -rf %s.py", runID);
				execute_cmd(cmd);
				update_result_mysql(runID, ACflag, max_time, max_memory);
				break;	
			}	
		}
	}
	
	// update result
	if (ACflag == OJ_AC) {
		// delete files
		char cmd[105] = {0};
		sprintf(cmd, "rm -rf %s_out.out", runID);
		execute_cmd(cmd);

		memset(cmd, 0, sizeof(cmd));
		sprintf(cmd, "rm -rf %s_error.txt", runID);
		execute_cmd(cmd);
		
		// 删除生成的可执行文件
		memset(cmd, 0, sizeof(cmd));
		sprintf(cmd, "rm -rf %s", runID);
		execute_cmd(cmd);

		memset(cmd, 0, sizeof(cmd));
		if (lang == 1)
			sprintf(cmd, "rm -rf %s.c", runID);
		else if (lang == 2)
			sprintf(cmd, "rm -rf %s.cpp", runID);
		else
			sprintf(cmd, "rm -rf %s.py", runID);
		execute_cmd(cmd);
		update_result_mysql(runID, ACflag, max_time, max_memory);
	}


	while(n--){
        //remove(s[n]);
        delete []s[n];
    }

	return 0;
}
