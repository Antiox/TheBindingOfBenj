using GameLibrary;
using UnityEngine;

public class PlayerVision2DScript : MonoBehaviour
{
    [SerializeField, Range(0,360), Tooltip("Champ de vision en degr�s")] 
    private float fieldOfView = 90f;

    [SerializeField, Range(5, 1000), Tooltip("Nombre de raycast pour la v�rification d'obstacles. Plus c'est �lev�, plus c'est pr�cis")] 
    private float raycastCount = 5f;

    [SerializeField, Range(1, 100), Tooltip("Distance maximale de la lampe torche")] 
    private float viewDistance = 30f;

    [SerializeField, Range(0, 10), Tooltip("Distance qui d�finit � quel point on voit au travers des obstacles")] 
    private float obstaclePenetration = 1f;

    [SerializeField, Tooltip("Masque qui indique quelles layers sont ignor�es pour les raycast de d�tection d'obstacles")]
    private LayerMask ignoreMask;

    // Mesh qui correspond � la zone d'affichage de la lampe torche
    private Mesh fovMesh;

    void Start()
    {
        fovMesh = new Mesh();
        fovMesh.name = "Fov Mesh";
        GetComponent<MeshFilter>().mesh = fovMesh;
    }

    void LateUpdate()
    {
        // R�cup�ration de la position de la souris
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Calcul du vecteur de direction du joueur vers la souris
        var direction = (mousePosition - transform.position).normalized * viewDistance;

        // Valeur symbolisant l'espace en degr�s entre deux raycast
        var step = fieldOfView / raycastCount;

        // Vertex et triangles du mesh que l'on va dessiner
        var vertices = new Vector3[(int)raycastCount + 1];
        var triangles = new int[(int)(raycastCount - 1) * 3];

        // Pour chaque raycast que l'on va envoy� (on part de (fov/2)� vers -(fov/2)�, pour centrer le fov au niveau de la souris.
        // Par exemple, pour un fov � 100�, on boucle de 50� � -50�
        for (int i = 0; i < raycastCount - 1; i++)
        {
            // Calcul du vecteur de direction
            var rayDirection = Utility.RotateVector2(direction, fieldOfView / 2 - step * i);

            // R�cup�ration du point d'impact du raycast
            var hit = Physics2D.Raycast(transform.position, rayDirection, direction.magnitude, ~ignoreMask);

            // Construction du mesh
            vertices[i + 1] = hit.collider is null ? rayDirection : (Vector2)transform.InverseTransformPoint(hit.point + rayDirection.normalized * obstaclePenetration);
            if (i < raycastCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        // Assignation des vertex et triangles au nouveau mesh
        fovMesh.Clear();
        fovMesh.vertices = vertices;
        fovMesh.triangles = triangles;
    }
}
