using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;       // Kecepatan gerakan
    [SerializeField] private float jumpForce = 5f;   // Kekuatan lompatan
    [SerializeField] private float rotationSpeed; // Kecepatan rotasi dalam derajat per detik
    private Rigidbody _rigidBody;
    [SerializeField] private bool isGrounded;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        // Opsional: Membekukan rotasi jika diperlukan
        // _rigidBody.freezeRotation = true;
    }

    private void Update()
    {
        // Cek apakah objek menyentuh tanah
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Menangani input lompatan
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            _rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Mendapatkan input horizontal dan vertikal
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Membuat vektor gerakan di sumbu X dan Z
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        // Set kecepatan langsung tanpa menggunakan gaya, lebih responsif
        _rigidBody.velocity = new Vector3(movement.x * speed, _rigidBody.velocity.y, movement.z * speed);

        //_rigidBody.velocity = new Vector3(moveHorizontal, _rigidBody.velocity.y, moveVertical) * speed;

        // Cek apakah ada pergerakan
        if (movement.magnitude > 0.1f)
        {
            // Hitung rotasi yang diinginkan
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);

            // Hitung rotasi baru dengan interpolasi untuk kelancaran
            Quaternion newRotation = Quaternion.RotateTowards(_rigidBody.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Terapkan rotasi ke Rigidbody
            _rigidBody.MoveRotation(newRotation);
        }
    }
}
