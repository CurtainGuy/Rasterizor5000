#version 330
 
// shader input
in vec2 vUV;				// vertex uv coordinate
in vec3 vNormal;			// untransformed vertex normal
in vec3 vPosition;			// untransformed vertex position

// shader output
out vec2 uv;	
out vec4 normal;			// transformed vertex normal
out vec4 worldPos;			// the world position which is used in te fragment shader

uniform mat4 transform;		// rotation
uniform mat4 toWorld;		// matrix to world space
 
// vertex shader
void main()
{
	// transform vertex using supplied matrix
	gl_Position = transform * vec4(vPosition, 1.0);

	// asing worldPos
	worldPos = toWorld * vec4( vPosition, 1.0f);

	// forward normal and uv coordinate; will be interpolated over triangle
	normal = toWorld * vec4( vNormal, 0.0f );
	normal = normalize(normal);
	uv = vUV;
}