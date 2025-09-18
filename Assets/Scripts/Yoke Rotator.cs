using UnityEngine;


public class YokeLookAtHand : MonoBehaviour
{
    [SerializeField] Transform hand;
    [SerializeField] Transform yoke;
    [SerializeField] float maxDegreesPerSecond = 720f; 
    [SerializeField] Vector3 forwardOffsetEuler = Vector3.zero;
    [SerializeField] float multiplie;

    private void FirstVariant()
    {
        if (hand == null) return;

        Vector3 worldDir = hand.position - transform.position;

        forwardOffsetEuler.z = worldDir.z * multiplie;

        worldDir.z = 0;

        if (worldDir.sqrMagnitude < 1e-6f) return;

        Quaternion targetRotation = Quaternion.LookRotation(worldDir.normalized, yoke.right);

        if (forwardOffsetEuler != Vector3.zero)
        {
            targetRotation *= Quaternion.Euler(forwardOffsetEuler);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesPerSecond * Time.deltaTime);

        float z = transform.rotation.eulerAngles.z;
        float x = transform.rotation.eulerAngles.x;

        Debug.Log(transform.rotation.eulerAngles);

        if ((z >= 0 && z <= 8) || (z >= 353 && z <= 360))
        {
            // в допустимой зоне, ничего не делаем
        }
        else
        {
            if (z < 180)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 8);
            }
            else transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 353);
        }
        if (Mathf.Clamp(x, 336, 356) != x)
        {
            if (Mathf.Clamp(x, 0, 180) == x)
            {
                transform.rotation = Quaternion.Euler(356, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                return;
            }
            transform.rotation = Quaternion.Euler(Mathf.Clamp(x, 336, 356), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }

    private void SecondVariant()
    {
        if (hand == null) return;

        Transform planeTransform = transform.parent; // Предполагаем, что родитель — самолёт
        if (planeTransform == null)
        {
            // Если родителя нет, используем мировое пространство
            planeTransform = transform.root;
        }

        // Преобразуем позицию руки в локальное пространство самолёта
        Vector3 localHandPos = planeTransform.InverseTransformPoint(hand.position);
        Vector3 localYokePos = yoke.localPosition; // Обычно Vector3.zero, если штурвал в начале координат

        Vector3 localDir = localHandPos - localYokePos;
        float multiply = multiplie;

        forwardOffsetEuler.z = localDir.z * multiply; // Локальный Z для смещения вперёд

        localDir.z = 0; // Игнорируем локальную глубину для горизонтального управления

        if (localDir.sqrMagnitude < 1e-6f) return;

        Quaternion targetLocalRotation = Quaternion.LookRotation(localDir.normalized, Vector3.right + new Vector3(0, 0, 90)); // Локальный "верх" для стабильного вращения

        if (forwardOffsetEuler != Vector3.zero)
        {
            targetLocalRotation *= Quaternion.Euler(forwardOffsetEuler);
        }

        // Плавный поворот локальной ориентации
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetLocalRotation, maxDegreesPerSecond * Time.deltaTime);

        Vector3 localEulers = transform.localEulerAngles; // Ограничиваем локальные углы
        Debug.Log("Локальные углы Эйлера: " + localEulers); // Логируем локальные углы для отладки

        float localZ = localEulers.z;
        float localX = localEulers.x;

        // Ограничение Z (рыскание/крен: ~ -7°...+8°, так как 353° ≈ -7°)
        if (!((localZ >= 0f && localZ <= 8f) || (localZ >= 353f && localZ <= 360f)))
        {
            float clampedZ;
            if (localZ < 180f)
            {
                clampedZ = 8f;
            }
            else
            {
                clampedZ = 353f; // Или 352f для -8°
            }
            transform.localRotation = Quaternion.Euler(localX, localEulers.y, clampedZ);
        }

        // Ограничение X (тангаж: 336°-356° ≈ -24°...-4°)
        if (Mathf.Clamp(localX, 336f, 356f) != localX)
        {
            float clampedX;
            if (Mathf.Clamp(localX, 0f, 180f) == localX)
            {
                clampedX = 356f;
                transform.localRotation = Quaternion.Euler(clampedX, localEulers.y, localEulers.z);
                return;
            }
            clampedX = Mathf.Clamp(localX, 336f, 356f);
            transform.localRotation = Quaternion.Euler(clampedX, localEulers.y, localEulers.z);
        }
    }

    private void ThirdVariant()
    {
        Vector3 handPosition = transform.parent.InverseTransformDirection(hand.position);
        transform.LookAt(hand, yoke.right);
        transform.rotation *= Quaternion.Euler(forwardOffsetEuler);        
    }
    void LateUpdate()
    {
        ThirdVariant();
    }
}

