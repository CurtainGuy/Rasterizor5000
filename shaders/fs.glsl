#version 330
in vec2 uv;                 // interpolated texture coordinates
in vec4 normal;             // interpolated normal, world space
in vec4 worldPos;           // world space position of fragment
uniform sampler2D pixels;   // texture sampler
out vec4 outputColor;       // shader output
uniform vec3 lightPos;      // light position in world space
void main()                 // fragment shader
{
	vec3 L = lightPos - worldPos.xyz;
	float dist = length(L);
	L = normalize( L );
	vec3 lightColor = vec3( 100, 100, 100);
	vec3 materialColor = texture( pixels, uv).xyz;
	vec3 ambientColor = vec3(0.0f, 0.0f, 0.0f);
	float attenuation = 1.0f / (dist * dist);
	vec3 Phong = vec3(pow(max(0.0f, dot(L, lightPos)), 1));
	outputColor = vec4( ambientColor + 
		materialColor 
		//+ Phong * attenuation
		* max( 0.0f, dot( L, normal.xyz) ) * attenuation * lightColor
		, 1);
}