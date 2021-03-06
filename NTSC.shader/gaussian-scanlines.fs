// NTSC Shader - written by Hans-Kristian Arntzen
// License: GPLv3
// pulled from git://github.com/libretro/common-shaders.git on 01/30/2014

#version 150

#define DISPLAY_GAMMA 2.1
#define CRT_GAMMA 2.5
#define TEX(off) pow(texture(source[0], texCoord + vec2(0.0, (off) * one.x)).rgb, vec3(CRT_GAMMA)) //this originally uses one.y, but that makes weird lines for some reason :/

uniform sampler2D source[];
uniform vec4 sourceSize[];
uniform vec4 targetSize;

in Vertex {
  vec2 texCoord;
};

out vec4 fragColor;

void main() {
vec2 pix_no = texCoord * sourceSize[0];
vec2 one = 1.0 / sourceSize[0];

   vec3 frame0 = TEX(-2.0);
   vec3 frame1 = TEX(-1.0);
   vec3 frame2 = TEX(0.0);
   vec3 frame3 = TEX(1.0);
   vec3 frame4 = TEX(2.0);
   
   float offset_dist = fract(pix_no.y) - 0.5;
   float dist0 =  2.0 + offset_dist;
   float dist1 =  1.0 + offset_dist;
   float dist2 =  0.0 + offset_dist;
   float dist3 = -1.0 + offset_dist;
   float dist4 = -2.0 + offset_dist;
   
   vec3 scanline = frame0 * exp(-5.0 * dist0 * dist0);
   scanline += frame1 * exp(-5.0 * dist1 * dist1);
   scanline += frame2 * exp(-5.0 * dist2 * dist2);
   scanline += frame3 * exp(-5.0 * dist3 * dist3);
   scanline += frame4 * exp(-5.0 * dist4 * dist4);
   
   fragColor = vec4(pow(1.15 * scanline, vec3(1.0 / DISPLAY_GAMMA)), 1.0);
}