#include <stdio.h>
#include <fcntl.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <unistd.h>
#include <string.h>


int main()
{
	int end = 0;
	int cTos;
	int sToc; 
	int pid;
	char str[BUFSIZ]; 
   	/* write str to the FIFO */
   	cTos = open("ctosPipe", O_WRONLY);
   	sToc = open("stocPipe", O_RDONLY);
   	if(cTos < 0 && sToc < 0){
		printf("Please start the server first");
		return 0;
   	}   	
	while(1){	
   		printf("Client: ");
		fgets(str,sizeof(str),stdin);
		write(cTos, str, sizeof(str));
		if(strcmp(str,"exit")==0 || strcmp(str,"exit\n")==0){
			break;
		} 
		if(read(sToc,str,sizeof(str))>0){
			printf("Server: %s",str);
		}
		if(strcmp(str,"exit")==0 || strcmp(str,"exit\n")==0){
			break;
		}
		/* clean buf from any data */
      		memset(str, 0, sizeof(str));
	}
	printf("The server left the conversation! Stopping client...\n");	
	close(sToc);
	close(cTos);
	return 0;
}
