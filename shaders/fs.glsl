#version 330
in vec2 uv;                 // interpolated texture coordinates
in vec4 normal;             // interpolated normal, world space
in vec4 worldPos;           // world space position of fragment

out vec4 outputColor;       // shader output

uniform vec3 lightPos;      // light position in world space
uniform vec3 lightColor;	// color of the light
uniform vec3 ambientColor;	// color of the ambient light
uniform sampler2D pixels;   // texture sampler

void main()                 // fragment shader
{
	vec3 L = lightPos - worldPos.xyz;
	float dist = length(L);
	L = normalize(L);
	vec3 R = 2 * dot(L, normal.xyz) * normal.xyz - L;
	vec3 materialColor = texture( pixels, uv).xyz;
	float attenuation = 1.0f / (dist * dist);
	vec3 Phong = ambientColor + materialColor * pow(max( 0.0f, dot( L, R)), 100) * lightColor;
	outputColor = vec4( (materialColor * max( 0.0f, dot( L, normal.xyz) ) * lightColor + Phong) * attenuation, 1); // Normal + phong
	//outputColor = vec4( (materialColor * max( 0.0f, dot( L, normal.xyz) ) * lightColor) * attenuation, 1); // Normal only
	//outputColor = vec4(Phong * attenuation,1); // Phong only
}