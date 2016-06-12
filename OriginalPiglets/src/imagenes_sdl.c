/*
 *      imagenes_sdl.c
 *      
 *      Copyright 2009 Banshuu <franrulez@gmail.com>
 *      
 *      This program is free software; you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation; either version 2 of the License, or
 *      (at your option) any later version.
 *      
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *      
 *      You should have received a copy of the GNU General Public License
 *      along with this program; if not, write to the Free Software
 *      Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 *      MA 02110-1301, USA.
 */

#include "SDL/SDL.h" 
#include "SDL/SDL_thread.h"
#include "SDL/SDL_image.h"
#include "SDL/SDL_mixer.h"
#include "SDL/SDL_ttf.h"

#ifndef IMAGENES_SDL_C
#define IMAGENES_SDL_C

//bool
#define FALSE 0
#define TRUE 1

//La pantalla
const int SCREEN_WIDTH = 640; 
const int SCREEN_HEIGHT = 480; 
const int SCREEN_BPP = 32; 


TTF_Font *font = NULL; 
SDL_Color textColor = { 0, 0, 0 }; 


SDL_Surface *screen = NULL; 

//carga una imagen optimizada en una SDL_Surface
SDL_Surface 
*load_image( char* filename ) 
{ 
	SDL_Surface* loadedImage = NULL; 
	SDL_Surface* optimizedImage = NULL; 
	
	loadedImage = IMG_Load( filename ); 
	if( loadedImage != NULL ) 
	{ 
		//if(IMG_isPNG(loadedImage))
		//{
			optimizedImage = SDL_DisplayFormatAlpha( loadedImage ); 
		//}
		//else
		//{
			//optimizedImage = SDL_DisplayFormat( loadedImage ); 
		//}
		SDL_FreeSurface( loadedImage ); 
	} 
	 
	//if( optimizedImage != NULL )
	//{ 
		//Uint32 colorkey = SDL_MapRGB( optimizedImage->format, 0, 255, 72 );
		//SDL_SetColorKey( optimizedImage, SDL_SRCCOLORKEY, colorkey );
	//}
	
	return optimizedImage;
}


//le pone texto a una SDL_Surface, tipo de font: arsle gothic
SDL_Surface 
*apply_Text( char *texto, int size  )
{
	if(font != NULL ){ TTF_CloseFont( font ); }
	font = TTF_OpenFont( "Arsle Gothic.ttf", size ); 
	SDL_Surface *surf=NULL;
	if( font == NULL ) { return NULL; } 
	surf = TTF_RenderText_Solid( font, texto, textColor );
	if(surf == NULL) { return NULL; } 
	return surf;
}


//pone una surface en otra surface, en las coordenadas x,y
void 
apply_surface( int x, int y, SDL_Surface* source, SDL_Surface* destination ) 
{ 
	SDL_Rect offset;  
	offset.x = x; offset.y = y; 
	SDL_BlitSurface( source, NULL, destination, &offset ); 
} 

void 
clean_up() 
{ 
	TTF_CloseFont( font ); 
	TTF_Quit();  
	printf("Quiting SDL.\n"); 
	Mix_CloseAudio(); 
	SDL_Quit(); 
}

//inicializa la sdl
int 
init()
{
	printf("Vamos a hacer piglets!!!\n");
	/* Initialize defaults, Video and Audio */
    if((SDL_Init(SDL_INIT_EVERYTHING)==-1)) { 
        printf("Could not initialize SDL: %s.\n", SDL_GetError());
        return FALSE;
    }
	screen = SDL_SetVideoMode( SCREEN_WIDTH, SCREEN_HEIGHT, SCREEN_BPP, SDL_SWSURFACE /*| SDL_FULLSCREEN*/ ); 
	if( screen == NULL ) { return FALSE; }
	
	//Initialize SDL_mixer 
	if( Mix_OpenAudio( MIX_DEFAULT_FREQUENCY, MIX_DEFAULT_FORMAT, 2, 4096 ) == -1 ) { 
		 printf("Mix_OpenAudio: %s\n", Mix_GetError());
		return FALSE; 
	}
	
	if( TTF_Init() == -1 ) { return FALSE; }  
	
	
	printf("SDL Inicializada\n");
	
	//Set the window caption 
	SDL_WM_SetCaption( "Piglets", NULL ); 
	
	return TRUE;
}


#endif
