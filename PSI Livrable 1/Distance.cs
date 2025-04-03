import math
namespace PSI_Livrable_1
{
# Fonction de calcul de la distance avec la formule de Haversine
    def haversine(lat1, lon1, lat2, lon2):
    R = 6371  # Rayon moyen de la Terre en km
    phi1, phi2 = math.radians(lat1), math.radians(lat2)
    delta_phi = math.radians(lat2 - lat1)
    delta_lambda = math.radians(lon2 - lon1)

    a = math.sin(delta_phi / 2.0) ** 2 + math.cos(phi1)* math.cos(phi2) * math.sin(delta_lambda / 2.0) ** 2
    c = 2 * math.atan2(math.sqrt(a), math.sqrt(1 - a))

    return R* c  # Distance en km
}