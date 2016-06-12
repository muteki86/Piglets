CC=gcc
CFLAGS=-O2 -Wall
LIBS=-lSDLmain -lSDL -lSDL_mixer -lSDL_image -lSDL_ttf

all: main.c
	$(CC) $(CFLAGS) main.c -o piglets $(LIBS) 

clean:
	#rm *.o
	rm piglets
