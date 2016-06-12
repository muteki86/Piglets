/*
 *      main.c
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

/*
 * - mapa de 360x420 -> 36x42 cuadros de 10x10, empieza en el pixel 27x30 - 27x450 - 386x450 - 386x30
 */

#include <stdio.h>

#include "imagenes_sdl.c" 
#include "game.c"

int main(int argc, char** argv)
{
	if( init()== FALSE)
	{ 
		return 1; 
	}
	show_presentacion();
	clean_up();
	return 0;
}
