#include <fcntl.h>
#include <stdio.h>
#include <sys/stat.h>
#include <unistd.h>
#include <string.h>

int main()
{
   	int cTos;
   	int sToc;
   	char str[BUFSIZ];

   	/* create the FIFO (named pipe) */
   	mkfifo("ctosPipe", 0666);
   	mkfifo("stocPipe", 0666);
	printf("Waiting for client...\n");

  	/* open, read, and display the message from the FIFO */
   	cTos = open("ctosPipe", O_RDONLY);
   	sToc= open("stocPipe", O_WRONLY);

   	printf("Server started.\n");

  	while (1)
   	{
		if(read(cTos,str,sizeof(str))>0){
			printf("Client: %s",str);
		}	
		if(strcmp(str,"exit")==0 || strcmp(str,"exit\n")==0){
			break;
		} 	
		memset(str, 0, sizeof(str));
      		printf("Server: ");
		fgets(str,sizeof(str),stdin);
		write(sToc, str, sizeof(str));
		if(strcmp(str,"exit")==0 || strcmp(str,"exit\n")==0){
			break;
		}		     		
   	}
	printf("The client left the conversation! Stopping server...\n");	
	close(cTos);
   	close(sToc);
   	unlink("ctosPipe");
   	unlink("stocPipe");
   	return 0;	
}
