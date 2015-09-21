#include <stdio.h>
#include <fcntl.h>
#include <time.h>
#include <unistd.h>
#define SIZE (1024*16) 
#define TOTAL (2147483648) //~2GB.

int main()
{
	clock_t start,end;
	start = clock();
	char buffer[SIZE];
	int writeCount = TOTAL/SIZE;
	int fd = open("myfile.txt", O_CREAT | O_TRUNC | O_RDWR, 0666);
	for(int i=0;i< writeCount; i++){
		write(fd, buffer, SIZE);
	}
	close(fd);
	end = clock();
	double executionTime = (double)(end - start)/CLOCKS_PER_SEC;
	printf("bs = %d, count= %d , time = %f seconds",SIZE,writeCount,executionTime);
	return 0;
}