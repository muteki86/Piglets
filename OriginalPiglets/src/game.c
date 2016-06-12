/*
 *      game.c
 *      
 *      Copyright 2009 Franrulez <franrulez@gmail.com>
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
#include <stdlib.h>
#include <time.h>
#include "imagenes_sdl.c"

#define PIGLET_SIZE 30 //tamaño de los sprites de los piglets
#define FOOD_SIZE 15 //tamaño de los sprites de la comida
#define LOG_SIZE 15 //tamano de los sprites de los logs

#define GEN_TIME 15 //tiempo que dura una generación 

#define FRAMES_PER_SECOND 5

//coordenadas en la pantalla donde comienza el mapa
#define mapa_x 27
#define mapa_y 30

struct _piglet {
  int _x;//posicion x del chancho
  int _y;//posicion y del chancho
  //cualidades. A DEFINIR: cantidades máximas de cada cosa
  int _inteligencia;//inteligencia del chancho aka cantidad de veces que cambia de direccion
  int _vel_run;//velocidad del chancho cuando corre
  int _vel_walk;//velocidad del chancho cuando camina
  int _stamina;//stamina: cantidad de tiempo que puede correr
  int _color;//color?
  int _vision;//rango de vision del chancho
  int _direccion; //direccion en la que va el piglet
  char id[3];
  int tipo;//tipo de chancho al pintar :p
  
  int _eaten;//para saber cuanto se ha comido
  
} _piglet;

struct _food{
	int _x;
	int _y;
}_food;

struct _log{
	int _x;
	int _y;
}_log;

//Variables!
SDL_Surface *background = NULL;
SDL_Surface *chancho_u = NULL;
SDL_Surface *chancho_d = NULL;
SDL_Surface *chancho_l = NULL;
SDL_Surface *chancho_r = NULL;
SDL_Surface *comida = NULL;
SDL_Surface *_madera = NULL;
SDL_Surface *_tiempo = NULL;
SDL_Surface *mensaje2 = NULL;//para imprimir el número de generación
SDL_Surface *mensaje1 = NULL;//Dice "Generación No. :"
SDL_Surface *ids=NULL;
Mix_Music *music = NULL;
struct _food *f = NULL;
struct _piglet *pig1 = NULL;
struct _log *logs = NULL;
int Bool = FALSE;
unsigned _f = 0;//para los eventos...
unsigned _l = 0;//para los eventos...
Uint32 start_time = 0;//para el tiempo de juego :p
unsigned int gen_number = 0;//numero de generación por la que se va
unsigned int random_food = FALSE;//si la comida se pone al azar 

//las funciones!
void show_main();
void show_opciones();
void set_options();
void init_game();
void play_music();
void move_piglet(struct _piglet *p, const unsigned dir, unsigned int _vel);
int check_state_pigs( void *data);
void pintar_mapa(struct _piglet *p, struct _food *f);
void pintar_piglet(struct _piglet *p, const unsigned dir);
void poner_comida(const unsigned x, const unsigned y);
void set_comida_logs();
void set_chanchos();
int check_events(void *data);
void poner_logs(const unsigned x, const unsigned y);
int check_colisiones( struct _piglet *p, const unsigned _xn, const unsigned _yn );
int check_colisiones_logs_pigs( const unsigned _x, const unsigned _y );
int check_colisiones_logs( const unsigned _x, const unsigned _y );
int check_col(SDL_Rect A, SDL_Rect B);
int check_for_food(struct _piglet *p);
void check_eaten(struct _piglet *p);
int check_time();
void endless_game();
void select_piglets(void *data);
void create_piglet( const struct _piglet *parent1, const struct _piglet *parent2, struct _piglet *child, const unsigned int i );
void free_all();
void pintar_id();
void mutate(const unsigned no);
void set_food_random();

/*
 * 
 * 	PANTALLAS
 * 
 */
void 
show_presentacion()
{
	SDL_Surface *message = NULL;
	int i = 50;
	message = load_image( "img/banshuuLogo.bmp" );
	background = load_image( "img/fondo.bmp" );  
	
	music = Mix_LoadMUS("snd/intro.wav");
	if( music == NULL ) { 
		printf("No music");
	} 
	 Mix_VolumeMusic(80);
	 
	 Uint32 tiempo = SDL_GetTicks();
	 
	if( Mix_PlayMusic( music, 1 ) == -1 ) { printf("No music 2"); }
	while (i<SDL_ALPHA_OPAQUE )
	{
		SDL_SetAlpha(message, SDL_SRCALPHA, i); 
		apply_surface( 0, 0, background, screen );
		apply_surface( 210, 150, message, screen );  
		SDL_Event ev;
		while( SDL_PollEvent( &ev ) ) 
		{
				if( ev.type == SDL_QUIT ){exit(0);}
		}
		SDL_Delay(1);
		SDL_Flip( screen );
		i += 1;
	}
	
	if( (tiempo - SDL_GetTicks()) < 3000 )
	{
		SDL_Delay( ( 3000 - (tiempo-SDL_GetTicks()) ) );
	}
	
	i = SDL_ALPHA_OPAQUE;
	while (i>SDL_ALPHA_TRANSPARENT )
	{
		SDL_SetAlpha(message, SDL_SRCALPHA, i); 
		apply_surface( 0, 0, background, screen );
		apply_surface( 210, 150, message, screen ); 
		 
		SDL_Event ev;
		while( SDL_PollEvent( &ev ) ) 
		{
				if( ev.type == SDL_QUIT ){exit(0);}
		}
		SDL_Flip( screen );
		i -= 10;
	}
	SDL_Delay(200);
	if ( message != NULL ) { SDL_FreeSurface( message ); } 
	if ( background != NULL ) { SDL_FreeSurface( background ); }
	if ( music != NULL ) { Mix_FreeMusic( music ); }
	show_main();
}

void 
seleccionar_opcion(const unsigned int opcion, SDL_Surface *cuadro)
{
	switch (opcion)
	{
		case 1:
			apply_surface( 0, 0, background, screen );
			apply_surface( 220, 155, cuadro, screen );
			break;
		case 2:
			apply_surface( 0, 0, background, screen );
			apply_surface( 220, 220, cuadro, screen );
			break;
		case 3:
			apply_surface( 0, 0, background, screen );
			apply_surface( 220, 290, cuadro, screen );
			break;
		case 4:
			apply_surface( 0, 0, background, screen );
			apply_surface( 220, 371, cuadro, screen );
			break;
	}
				
	SDL_Flip( screen );
}

//muestra los créditos
void 
show_creditos()
{
	int _b2 = FALSE;
	background = load_image( "img/fondo_creditos.jpg" );
	if (background == NULL)
	{
		Bool = TRUE;
	}
	else
	{
		apply_surface( 0, 0, background, screen );
		
		SDL_Event ev;
		SDL_Flip( screen );
		while ( _b2 == FALSE)
		{
			while( SDL_PollEvent( &ev ) ) 
			{
				if( ev.type == SDL_QUIT )
				{
					Bool = TRUE;
					_b2 = TRUE;
				}
				if(ev.type == SDL_KEYDOWN)
				{
					_b2 = TRUE;
				}
				if(ev.type == SDL_MOUSEBUTTONDOWN)//mouse click
				{
					_b2=TRUE;
				}
			}
			SDL_Delay(10);
		}
		if (background!=NULL) {SDL_FreeSurface(background);}
	}
}

//muestra la pantalla principal con las opciones
void 
show_main()
{
	if(!Bool)
	{
		SDL_Surface *cuadro = NULL;
		unsigned int opcion = 1;
		background = load_image( "img/piglets_main.jpg" );
		cuadro = load_image("img/main_box.png");
		
		if( (background !=NULL) && (cuadro != NULL) )
		{
			seleccionar_opcion(opcion, cuadro);
			SDL_Event ev;
			play_music();
	
			int _b1 = FALSE;
			while ( _b1 == FALSE)//escoger
			{
				SDL_Delay(10);
				
				while( SDL_PollEvent( &ev ) ) {
					if( ev.type == SDL_QUIT ){
						Bool=TRUE;
						_b1= TRUE;
					}
					if(ev.type == SDL_MOUSEMOTION)//cambiar el cuadro dependiendo de donde esté el mouse
					{
						int _c = FALSE;
						SDL_Rect mouse;
						SDL_Rect _cuadro;
						mouse.x = ev.motion.x;
						mouse.y = ev.motion.y;
						mouse.h = 10;
						mouse.w = 10;
						//primer cuadro
						_cuadro.x = 220; _cuadro.y = 155; _cuadro.h = 45; _cuadro.w = 145;
						if (check_col(mouse, _cuadro))
						{
							_c = TRUE;
							seleccionar_opcion(1, cuadro);
							opcion = 1;
						}
						//segundo cuadro
						_cuadro.x = 220; _cuadro.y = 220; _cuadro.h = 45; _cuadro.w = 145;
						if( !_c && check_col(_cuadro, mouse))
						{
							_c = TRUE;
							seleccionar_opcion(2, cuadro);
							opcion = 2;
						}
						//tercer cuadro
						_cuadro.x = 220; _cuadro.y = 290; _cuadro.h = 45; _cuadro.w = 145;
						if( !_c && check_col(mouse, _cuadro))
						{
							_c = TRUE;
							seleccionar_opcion(3, cuadro);
							opcion = 3;
						}
						//4rto cuadro
						_cuadro.x = 220; _cuadro.y = 371; _cuadro.h = 45; _cuadro.w = 145;
						if( !_c && check_col(mouse, _cuadro))
						{
							_c = TRUE;
							seleccionar_opcion(4, cuadro);
							opcion = 4;
						}
						
					}
					if(ev.type == SDL_MOUSEBUTTONDOWN)//mouse click
					{
						_b1=TRUE;
					}
					if(ev.type == SDL_KEYDOWN)
					{
						switch( ev.key.keysym.sym ) 
						{
							case SDLK_UP: 
								 if( opcion > 0)
								{
									--opcion;
									seleccionar_opcion(opcion, cuadro);
								}
								break;
							case SDLK_DOWN:
								if( opcion<4)
								{
									++opcion;
									seleccionar_opcion(opcion, cuadro);
								}
								break; 
								case SDLK_RETURN:
									_b1 = TRUE;
									break;
								default: break;
						}
					}
				}
			}
		}
		if (background!=NULL) {SDL_FreeSurface(background);}
		if (cuadro!=NULL) {SDL_FreeSurface(cuadro);}
		//ve que se escogió
		if (!Bool)
		{
			switch (opcion)
			{
				case 1:
					init_game();
					endless_game();
					free_all();//liberar memoria
					break;
				case 3:
					show_opciones();
					break;
				case 4:
					show_creditos();
					break;
				default:
					show_presentacion();
					break;
			}
		}
		if(!Bool){ show_main(); }
	}
}


void 
show_opciones()
{
	int _b2 = FALSE;
	background = load_image( "img/fondo_opciones.jpg" );
	if (background == NULL)
	{
		Bool = TRUE;
	}
	else
	{
		apply_surface( 0, 0, background, screen );
		
		SDL_Event ev;
		SDL_Flip( screen );
		while ( _b2 == FALSE)
		{
			while( SDL_PollEvent( &ev ) ) 
			{
				if( ev.type == SDL_QUIT )
				{
					Bool = TRUE;
					_b2 = TRUE;
				}
				if(ev.type == SDL_KEYDOWN)
				{
					_b2 = TRUE;
				}
				if(ev.type == SDL_MOUSEBUTTONDOWN)//mouse click
				{
					_b2=TRUE;
				}
			}
			SDL_Delay(10);
		}
		if (background!=NULL) {SDL_FreeSurface(background);}
	}
}


/*
 * 
 * MÚSICA & EFECTOS
 * 
 */
//pone a sonar musica aleatoria entre las 4 que hay
void 
play_music()
{
	//Poner alguna música al random 
	switch((rand()%4)+1)
	{
		case 1:
			music = Mix_LoadMUS( "snd/music1.wav" );
			break;
		case 2:
			music = Mix_LoadMUS( "snd/music2.wav" );
			break;
		case 3:
			music = Mix_LoadMUS( "snd/music3.wav" );
			break;
		case 4:
			music = Mix_LoadMUS( "snd/music4.wav" );
			break;
	}
	if( music == NULL ) { 
		printf("No music");
	} 
	 Mix_VolumeMusic(30);
	if( Mix_PlayMusic( music, -1 ) == -1 ) { printf("No music 2 game"); }
}

/*
 * 
 * LOGICA DEL JUEGO
 * 
 */
//inicializa las variables del juego
void 
init_game()
{
	if(!Bool)
	{
		set_chanchos();
		set_comida_logs();
		srand ( time(NULL) );
		play_music();

		background = load_image( "img/lauyout_idea.jpg" );
		_madera = load_image("img/logs.png");
		comida = load_image("img/food.png");
		mensaje2 = load_image( "img/mensaje2.png" );
		
		start_time = SDL_GetTicks();
	}
}


int 
check_time()
{
	//liberar memoria...
	if (mensaje1!=NULL){ SDL_FreeSurface(mensaje1);}
	if(_tiempo!=NULL){ SDL_FreeSurface(_tiempo);}
	
	Uint32 actual = (SDL_GetTicks()-start_time)/1000;

	if(actual > GEN_TIME)
	{ 
		//hacer los chequeos de el algoritmo genético :p
		gen_number++;
		select_piglets(NULL);
		unsigned int i;
		for(i=0;i<50;++i)
		{
			logs[i]._x = -1;
			logs[i]._y = -1;
		}
		start_time = SDL_GetTicks();
	}
	char gen[20];
	sprintf( gen, "%s%d", "Generacion No.: ", gen_number);
	mensaje1 = apply_Text(gen, 15 );
	//num_gen = apply_Text( gen, 15 );
	
	char time[2];
	sprintf( time, "%d", actual);
	if(time==NULL){printf("time==null");}
	_tiempo = apply_Text( time , 15);
	
	return 1;
}


void 
set_chanchos()
{
	srand ( time(NULL) );
	pig1 = malloc( 10 * sizeof(_piglet) );
	gen_number = 0;
	unsigned int i = 0;
	for(; i<10; ++i)
	{
		pig1[i]._x = ((rand()%(360-PIGLET_SIZE))+1) ;
		pig1[i]._y = ((rand()%(420-PIGLET_SIZE))+1);
		pig1[i]._vel_walk = ((rand()%10)+1);
		pig1[i]._vel_run = ((rand()%20)+pig1[i]._vel_walk);
		pig1[i]._vision = ((rand()%30)+pig1[i]._vel_run);
		
		pig1[i]._direccion = ((rand()%4)+1);
		pig1[i]._inteligencia = ((rand()%900)+100);
		pig1[i]._stamina = ((rand()%5)+1);
		pig1[i]._color = 0; //VER!
		pig1[i]._eaten = 0;

		pig1[i].tipo = rand()%4;

		//para el fuckin id
		sprintf( pig1[i].id, "%d", gen_number);
	}
}

void 
set_comida_logs()
{
	unsigned i = 0;
	f = malloc(20 * sizeof(_food));
	for(i=0; i<20; ++i)
	{
		f[i]._x = -1;
		f[i]._y = -1;
	}
	
	logs = malloc(50 * sizeof(_log));
	for(i=0; i<50; ++i)
	{
		logs[i]._x = -1;
		logs[i]._y = -1;
	}
}

void
set_food_random()
{
	unsigned int i;
	for(i=0;i<20;++i)
	{
			if( (f[i]._x ==-1) && (f[i]._y ==-1) )
			{
				f[i]._x = rand()%360+mapa_x;
				f[i]._y = rand()%420+mapa_y;
			}
	}
}

//manejo de eventos en el juego
int 
check_events(void *data)
{
	SDL_Event ev;
	while( SDL_PollEvent( &ev ) ) {
		if( ev.type == SDL_QUIT ){
			printf( ">El usuario quiere salir.\n" );
			printf( ">SDL Terminado.\n" );
			Bool=TRUE;
		}
		if(ev.type == SDL_KEYDOWN)
		{
			switch( ev.key.keysym.sym ) 
			{
				case SDLK_ESCAPE:
					printf("wtf  ");
					//free_all(); //TODO
					show_main();
					break;
				case SDLK_r:
					random_food=!random_food;
				default: break;
			}
		}
		if( ev.type == SDL_MOUSEBUTTONDOWN ) { 
			if( ev.button.button == SDL_BUTTON_LEFT ) 
			{ 
				if(_f>20)
				{
					_f=0;
					f[_f]._x = ev.button.x;
					f[_f]._y = ev.button.y;
					//pintar_mapa(pig1, f);
					_f++;
				}
				else
				{
					f[_f]._x = ev.button.x; 
					f[_f]._y = ev.button.y;
					//pintar_mapa(pig1, f);
					_f++;
				}
			}
			if(ev.button.button == SDL_BUTTON_RIGHT)
			{
				if(_l>50)
				{
					if(!check_colisiones_logs_pigs( ev.button.x, ev.button.y ) )
					{
							if(!check_colisiones_logs(ev.button.x, ev.button.y))
							{
								_l=0;
								logs[_l]._x = ev.button.x;
								logs[_l]._y = ev.button.y;
								_l++;
							}
						}
					}
					else
					{
						if(!check_colisiones_logs_pigs( ev.button.x, ev.button.y ) )
						{
							if(!check_colisiones_logs(ev.button.x, ev.button.y))
							{
								logs[_l]._x = ev.button.x; 
								logs[_l]._y = ev.button.y;
								_l++;
							}
						}
					}
				}
			}
		}
	return 0;
}

int 
check_state_pigs( void *data )
{
	unsigned int j = 1000;
	for(; j>=100; --j)
	{
		unsigned int i = 0;
		for(; i<10; ++i)
		{
			if(!Bool)
			{
				if(pig1[i]._inteligencia == j)
				{
					if ( check_for_food(&pig1[i]) )
					{
						move_piglet( &pig1[i], pig1[i]._direccion, FALSE );
						check_eaten( &pig1[i] );
					}
					else
					{
						move_piglet(&pig1[i], (rand()%4)+1, TRUE);
						pintar_mapa(pig1, f);
					}
				}
				if(random_food==TRUE)
				{
					set_food_random();
				}
			}
		}
		check_events(NULL);
	}
	return 0;
}

//busca por comida
int 
check_for_food(struct _piglet *p)
{
	SDL_Rect _boxP;
	switch(p->_direccion)
	{
		case 1://arriba
			_boxP.h = PIGLET_SIZE+p->_vision;
			_boxP.w = PIGLET_SIZE;
			_boxP.x = mapa_x + p->_x;
			_boxP.y = mapa_y + p->_y - p->_vision;
			break;
		case 2://abajo
			_boxP.h = PIGLET_SIZE+ p->_vision;
			_boxP.w = PIGLET_SIZE;
			_boxP.x = mapa_x+p->_x;
			_boxP.y = mapa_y+p->_y;
			break;
		case 3://derecha
			_boxP.h = PIGLET_SIZE;
			_boxP.w = PIGLET_SIZE+ p->_vision;
			_boxP.x = mapa_x+p->_x;
			_boxP.y = mapa_y+p->_y;
			break;
		case 4://izquierda
			_boxP.h = PIGLET_SIZE;
			_boxP.w = PIGLET_SIZE+ p->_vision;
			_boxP.x = mapa_x + p->_x - p->_vision;
			_boxP.y = mapa_y + p->_y;
			break;
		default:
			return FALSE;
	}
	//hacer un SDL_Rect en las coordenadas del piglet
	//SDL_FillRect( background, &_boxP, SDL_MapRGB( screen->format, 0xFF, 0xFF, 0xFF ) ); 
	int _b = FALSE;
	int i = 0;
	//hace rectángulos de los hongos
	while(i<20 && _b==FALSE )
	{
		if( f[i]._x !=-1 && f[i]._y !=-1 )
		{
			SDL_Rect _all;
			_all.x = f[i]._x;
			_all.y = f[i]._y;
			_all.h = FOOD_SIZE;
			_all.w = FOOD_SIZE;
			//si encuentra colisión, correr en la misma dirección que lleva
			//SDL_FillRect( background, &_all, SDL_MapRGB( screen->format, 0xFF, 0xFF, 0xFF ) ); 
			_b = check_col(  _all, _boxP);
		}
		++i;
	}
	return _b;
}
	
void 
check_eaten(struct _piglet *p)
{
	//hacer un SDL_Rect en las corrdenadas del p[i].
	SDL_Rect _boxP;
	_boxP.x = mapa_x+p->_x;
	_boxP.y = mapa_y+p->_y;
	_boxP.h = PIGLET_SIZE;
	_boxP.w = PIGLET_SIZE;
	
	int _b = FALSE;
	int i = 0;
	//hace rectángulos de la comida
	//SDL_FillRect( background, &_boxP, SDL_MapRGB( screen->format, 0xFF, 0xFF, 0xFF ) ); 
	while(i<20 && _b==FALSE )
	{
		if( f[i]._x !=-1 && f[i]._y !=-1 )
		{
			SDL_Rect _all;
			_all.x = f[i]._x;
			_all.y = f[i]._y;
			_all.h = FOOD_SIZE;
			_all.w = FOOD_SIZE;
			//si encuentra colisión, correr en la misma dirección que lleva
			//SDL_FillRect( background, &_all, SDL_MapRGB( screen->format, 0xFF, 0xFF, 0xFF ) ); 
			_b = check_col(  _all, _boxP);
			if ( _b == TRUE )
			{
				f[i]._x = -1;
				f[i]._y = -1;
				++p->_eaten;
			}
		}
		++i;
	}
}

//mueve el piglet p en la direccion dir, con la velocidad definida para el chancho.
//dir 1:arriba 2:abajo 3:derecha 4:izquierda
void 
move_piglet(struct _piglet *p, const unsigned dir, unsigned int _vel)
{
	unsigned int _vel1 = 0;
	unsigned int moved = FALSE;
	
	if( _vel )
	{
		_vel1 = p->_vel_walk;
	}else
	{
		_vel1 = p->_vel_run;
	}
	
	switch (dir)
	{
		case 1://pa arriba
			if ( p->_y > 0 && (check_colisiones(p , p->_x, (p->_y - _vel1) )==FALSE) )
			{
				p->_y = (p->_y - _vel1);
				p->_direccion = dir;
				moved = TRUE;
			}
			break;
		case 2://pa abajo
			if( (p->_y + _vel1) < (420-PIGLET_SIZE) && (check_colisiones(p , p->_x, (p->_y + _vel1) )==FALSE ) )
			{
				p->_y = (p->_y + _vel1);
				p->_direccion = dir;
				moved = TRUE;
			}
			break;	
		case 3://pa la derecha
			if ( (p->_x + _vel1) < (360-PIGLET_SIZE) && (check_colisiones(p, (p->_x + _vel1), p->_y ) ==FALSE ) )
			{
				p->_x = (p->_x + _vel1);
				p->_direccion = dir;
				moved = TRUE;
			}
			break;
		case 4://pa la izquierda
			if( p->_x > 0 && (check_colisiones(p , (p->_x - _vel1), p->_y )==FALSE) )
			{
				p->_x = (p->_x - _vel1);
				p->_direccion = dir;
				moved = TRUE;
			}
			break;
	}
	if(!moved)
	{
		move_piglet(p, (rand()%4)+1, TRUE);
	}
}

//checkea colisiones de un chancho p con id n en la direccion dir. 
int 
check_colisiones( struct _piglet *p, const unsigned _xn, const unsigned _yn )
{
	//hacer un SDL_Rect en las corrdenadas del p.
	SDL_Rect _boxP;
	_boxP.x = _xn+mapa_x;
	_boxP.y = _yn+mapa_y;
	_boxP.h = PIGLET_SIZE;
	_boxP.w = PIGLET_SIZE;
	
	int _b = FALSE;
	int i = 0;
	//hace rectángulos de los logs
	while(i<50 && _b==FALSE )
	{
		if( logs[i]._x !=-1 && logs[i]._y !=-1 )
		{
			SDL_Rect _all;
			_all.x = logs[i]._x;
			_all.y = logs[i]._y;
			_all.h = LOG_SIZE;
			_all.w = LOG_SIZE;
			_b = check_col(  _all, _boxP);
		}
		++i;
	}
	return _b;
}

//para ver si no se está poniendo un log encima de un chancho
int 
check_colisiones_logs_pigs( const unsigned _x, const unsigned _y )
{
	//hacer un SDL_Rect en las corrdenadas del p.
	SDL_Rect _boxP;
	_boxP.x = _x;
	_boxP.y = _y;
	_boxP.h = LOG_SIZE;
	_boxP.w = LOG_SIZE;
	//SDL_FillRect( background, &_boxP, SDL_MapRGB( screen->format, 0xFF, 0xFF, 0xFF ) ); 
	int _b = FALSE;
	int i = 0;
	//hace rectángulos de los logs
	while( i<10 && _b==FALSE )
	{
		if(!_b)
		{
			SDL_Rect _all;
			_all.x = pig1[i]._x+mapa_x;
			_all.y = pig1[i]._y+mapa_y;
			_all.h = PIGLET_SIZE;
			_all.w = PIGLET_SIZE;
			//SDL_FillRect( background, &_all, SDL_MapRGB( screen->format, 0xFF, 0xFF, 0xFF ) ); 
			_b = check_col(  _all, _boxP);
			++i;
		}
	}
	return _b;
}

int 
check_colisiones_logs( const unsigned _x, const unsigned _y )
{
	//hacer un SDL_Rect en las corrdenadas del p.
	SDL_Rect _boxP;
	_boxP.x = _x;
	_boxP.y = _y;
	_boxP.h = 10;
	_boxP.w = 10;
	//SDL_FillRect( background, &_boxP, SDL_MapRGB( screen->format, 0xFF, 0xFF, 0xFF ) ); 
	int _b = FALSE;
	int i = 0;
	//hace rectángulos de los logs
	while( i<50 && _b==FALSE )
	{
		if(!_b)
		{
			SDL_Rect _all;
			_all.x = logs[i]._x;
			_all.y = logs[i]._y;
			_all.h = LOG_SIZE;
			_all.w = LOG_SIZE;
			//SDL_FillRect( background, &_all, SDL_MapRGB( screen->format, 0xFF, 0xFF, 0xFF ) ); 
			if( (_b = check_col(  _all, _boxP)) == TRUE )
			{
				logs[i]._x=-1;
				logs[i]._y=-1;
			}
			++i;
		}
	}
	return _b;
}

int 
check_col(SDL_Rect a, SDL_Rect b)
{
 	if(b.x + b.w < a.x)	return 0;
	if(b.x > a.x + a.w)	return 0;

	if(b.y + b.h < a.y)	return 0;
	if(b.y > a.y + a.h)	return 0;

	return 1;
}

void 
endless_game()
{
	if(!Bool)
	{
		pintar_mapa(pig1, f);
		Uint32 tiempo = 0;
		
		while( !Bool  ) 
		{
			tiempo = SDL_GetTicks();
			check_time();
			check_state_pigs(NULL);
			if ( SDL_GetTicks()-tiempo < 1000/FRAMES_PER_SECOND )
			{	
				SDL_Delay( ( 1000 / FRAMES_PER_SECOND ) - (SDL_GetTicks()-tiempo) );
			}
		}
	}
}


void 
free_all()
{
	if(f!=NULL) { 	free( f ); }
	if(pig1!=NULL) {	free(pig1); }
	if(logs!=NULL) {	free(logs); }
	if(music!=NULL) {	Mix_FreeMusic( music ); }
	if(comida!=NULL) {	SDL_FreeSurface( comida ); }
	if(_madera!=NULL) {	SDL_FreeSurface( _madera ); }
	if(chancho_u!=NULL) {	SDL_FreeSurface( chancho_u ); }
	if(chancho_d!=NULL) {	SDL_FreeSurface( chancho_d ); } 
	if(chancho_l!=NULL) {	SDL_FreeSurface( chancho_l ); } 
	if(chancho_r!=NULL) {	SDL_FreeSurface( chancho_r ); } 
	if(background!=NULL) {	SDL_FreeSurface( background ); }
	if(mensaje2!=NULL) {	SDL_FreeSurface( mensaje2 ); }
	if(mensaje1!=NULL) {	SDL_FreeSurface( mensaje1 ); }
	if(_tiempo!=NULL) {	SDL_FreeSurface( _tiempo ); }
	if(ids!=NULL){SDL_FreeSurface(ids);}
}

//selecciona a los chanchos más fittes :p
void 
select_piglets(void *data)
{
	//loop para sacar los chanchos que más han comido en un int[3] con los índices
	unsigned int elegidos[3];
	unsigned int _C;
	for(_C=0; _C < 3; ++_C){ elegidos[_C] = 0 ; }//inicializa elegidos
	unsigned int i;
	
	for(i=0;i<10;++i)
	{
		//agarrar el chancho, ver si _eaten es mayor que alguno de los 3 que hay en el "elegidos"
		//si lo es, mover los otros. El primero es el mayor.
		if( pig1[i]._eaten >= pig1[ elegidos[0] ]._eaten )
		{
			elegidos[2] = elegidos[1] ;
			elegidos[1] = elegidos[0] ;
			elegidos[0] = i ;
		}
		else
		{
			if( pig1[i]._eaten >= pig1[ elegidos[1] ]._eaten )
			{
				elegidos[2] = elegidos[1] ;
				elegidos[1] = i;
			}
			else
			{
				if( pig1[i]._eaten >= pig1[ elegidos[2] ]._eaten )
				{
					elegidos[2]=i;
				}
			}
		}
	}
	//si los 3 son iguales, se agarran 3 chanchos al azar
	if( (elegidos[0]==elegidos[1]) &&(elegidos[0]==elegidos[2]) && (elegidos[1]==elegidos[2]) )
	{
		elegidos[0]=rand()%10;
		elegidos[1]=rand()%10;
		elegidos[2]=rand()%10;
	}
	//loop para crear nuevos chanchos mezclando los anteriores.
	//usar rand()%3+1 para seleccionar los padres, tienen que ser 2 diferentes a huevo
	for( i=0; i<10; ++i)
	{
		if( (i!=elegidos[0]) && (i!=elegidos[1]) && (i!=elegidos[2])   )
		{
			unsigned int p1 = elegidos[rand()%3];
			unsigned int p2 = elegidos[rand()%3];
			create_piglet( &pig1[p1], &pig1[p2], &pig1[i], i );
		}
	}
	pig1[ elegidos[0] ]._eaten = 0;
	pig1[ elegidos[1] ]._eaten = 0;
	pig1[ elegidos[2] ]._eaten = 0;
	//ver si se muta, y cuantos se mutan
	if( (rand()%3)==0 )
	{
		mutate(rand()%4+1);
	}
}

//el create_piglet debería crear el chancho nuevo usando los atributos de los padre totalmente aleatoriamente,
//así, aunque sean los mismos padres, el hijo será diferente.
void 
create_piglet( const struct _piglet *parent1, const struct _piglet *parent2, 
										struct _piglet *child, const unsigned int i )
{
	struct _piglet ps[2];
	ps[0] = *parent1;
	ps[1] = *parent2;
	
	child->_x = ( (rand()%(360-PIGLET_SIZE))+1 ) ;
	child->_y = ( (rand()%(420-PIGLET_SIZE))+1 ) ;
	child->_vel_walk = ps[rand()%2]._vel_walk;
	child->_vel_run = ps[rand()%2]._vel_run;
	child->_vision = ps[rand()%2]._vision;
	child->_direccion = rand()%4+1;
	child->_inteligencia = ps[rand()%2]._inteligencia;
	child->_stamina = ps[rand()%2]._stamina;
	
	child->tipo = ps[rand()%2].tipo;
	
	sprintf(child->id, "%d", gen_number);

}

void
mutate(const unsigned n)
{
	unsigned int i ;
	for(i = 0; i < n; ++i)
	{
		unsigned int atrib = rand()%4;
		unsigned int pig = rand()%10;
		
		switch(atrib)
		{
			case 0:
				pig1[pig]._vel_walk = ((rand()%10)+1);
				break;
			case 1:
				pig1[pig]._vel_run = ((rand()%20)+pig1[pig]._vel_walk);
				break;
			case 2:
				pig1[pig]._vision = ((rand()%30)+pig1[pig]._vel_run);
				break;
			case 3:
				pig1[pig]._inteligencia =((rand()%900)+100);
				break;
		}
		unsigned int type = rand()%2;
		if(type == TRUE)
		{
			pig1[pig].tipo = rand()%4;
		}		
	}
}

/*
 * 
 * RENDERIZADO
 * 
 */
//pinta el mapa
void 
pintar_mapa(struct _piglet *p, struct _food *f)
{
	unsigned int i = 0;
	if(background !=NULL)
	{
		apply_surface( 0, 0, background, screen );

		for(i=0; i<10; ++i)
		{
			pintar_piglet( &p[i], p[i]._direccion );
		}
		for(i=0; i<20; ++i)
		{
			if( (f[i]._x != -1) &&( f[i]._y != -1 ) )
			{
				poner_comida(f[i]._x, f[i]._y);
			}
		}
		for(i=0; i<50; ++i)
		{
			if( (logs[i]._x != -1) &&( logs[i]._y != -1 ) )
			{
				poner_logs(logs[i]._x, logs[i]._y);
			}
		}
		//imprimir mensajes:
		if(_tiempo!=NULL) { apply_surface(590, 460, _tiempo, screen); }
		if(mensaje1!=NULL) { apply_surface (430, 60, mensaje1, screen); }
		if(mensaje2!=NULL) { apply_surface(430, 250, mensaje2, screen); }
		
		pintar_id();
		
		SDL_Flip( screen );
	}
	else
	{
		printf("error: fondo se fue a la mierda...");
	}
}

//pinta un chancho en el mapa
void 
pintar_piglet(struct _piglet *p, const unsigned dir)
{
	switch (dir)
	{
		case 1://pa arriba
			if(chancho_u!=NULL){SDL_FreeSurface(chancho_u);}
			switch ( p->tipo )
			{
				case 0:
					chancho_u = load_image("img/ptipo1_up.png");
					break;
				case 1:
					chancho_u = load_image("img/ptipo2_up.png");
					break;
				case 2:
					chancho_u = load_image("img/ptipo3_up.png");
					break;
				case 3:
					chancho_u = load_image("img/ptipo4_up.png");
					break;
			}
			apply_surface( (mapa_x+p->_x) , (mapa_y+p->_y) , chancho_u, screen);
			
			break;
		case 2://pa abajo
			if(chancho_d!=NULL){SDL_FreeSurface(chancho_d);}
			switch ( p->tipo )
			{
				case 0:
					chancho_d = load_image("img/ptipo1_down.png");
					break;
				case 1:
					chancho_d = load_image("img/ptipo2_down.png");
					break;
				case 2:
					chancho_d = load_image("img/ptipo3_down.png");
					break;
				case 3:
					chancho_d = load_image("img/ptipo4_down.png");
					break;
			}
			apply_surface( (mapa_x+p->_x) , (mapa_y+p->_y) , chancho_d, screen);
			break;	
		case 3://pa la derecha
			if(chancho_r!=NULL){SDL_FreeSurface(chancho_r);}
			switch ( p->tipo )
			{
				case 0:
					chancho_r = load_image("img/ptipo1_right.png");
					break;
				case 1:
					chancho_r = load_image("img/ptipo2_right.png");
					break;
				case 2:
					chancho_r = load_image("img/ptipo3_right.png");
					break;
				case 3:
					chancho_r = load_image("img/ptipo4_right.png");
					break;
			}
			apply_surface( (mapa_x+p->_x) , (mapa_y+p->_y) , chancho_r, screen);
			break;
		case 4://pa la izquierda
			if(chancho_l!=NULL){SDL_FreeSurface(chancho_l);}
			switch ( p->tipo )
			{
				case 0:
					chancho_l = load_image("img/ptipo1_left.png");
					break;
				case 1:
					chancho_l = load_image("img/ptipo2_left.png");
					break;
				case 2:
					chancho_l = load_image("img/ptipo3_left.png");
					break;
				case 3:
					chancho_l = load_image("img/ptipo4_left.png");
					break;
			}
			apply_surface( (mapa_x+p->_x) , (mapa_y+p->_y) , chancho_l, screen);
			break;
	}

}

//pinta la comida en el mapa
void 
poner_comida(const unsigned x, const unsigned y)
{	
	if ( (x > mapa_x) && (x < ((mapa_x+360)-FOOD_SIZE) ) )
	{
		if ( (y>mapa_y) && (y< ((mapa_y+420)-FOOD_SIZE) ) )
		{
			if(comida!=NULL)
			{
				apply_surface( x , y , comida, screen);
			}
		}
	}
}

void 
poner_logs(const unsigned x, const unsigned y)
{
	if ( (x > mapa_x) && (x < ((mapa_x+360)-LOG_SIZE) ) )
	{
		if ( (y>mapa_y) && (y< ((mapa_y+420)-LOG_SIZE) ) )
		{
			if(_madera!=NULL)
			{
				apply_surface( x , y , _madera, screen);
			}
		}
	}
}

void 
pintar_id()
{	
	unsigned int i;
	//pone los id's
	for(i=0; i<10;++i)
	{
		if(ids!=NULL){ SDL_FreeSurface( ids ); }
		ids = apply_Text( pig1[i].id, 16 );
		if(ids!=NULL)
		{
			apply_surface( (pig1[i]._x+mapa_x), (pig1[i]._y+mapa_y)-15, ids, screen);
		}
		else{ printf("fuck_%d  ", i); }
	}
}
