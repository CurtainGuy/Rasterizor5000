/* #version 330
in vec2 uv;                 // interpolated texture coordinates
in vec4 normal;             // interpolated normal, world space
in vec4 worldPos;           // world space position of fragment
uniform sampler2D pixels;   // texture sampler
out vec4 outputColor;       // shader output
uniform vec3 lightPos;      // light position in world space
void main()                 // fragment shader
{
	vec3 L = lightPos - worldPos.xyz;
	float dist = L.length();
	L = normalize( L );
	vec3 lightColor = vec3( 10, 10, 8 );
	vec3 materialColor = texture( pixels, uv).xyz;
	vec3 ambientColor = vec3(0.1f, 0.1f, 0.15f);
	float attenuation = 1.0f / (dist * dist);
	outputColor = vec4( ambientColor + 
		materialColor * max( 0.0f, dot( L, normal.xyz) ) * attenuation * lightColor, 1// +
		//pow(max( 0.0f, dot(L, lightPos)), 1)
		);
} */
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
uniform sampler2D pixels;		// texture sampler

// shader output
out vec4 outputColor;

// fragment shader
void main()
{outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );}