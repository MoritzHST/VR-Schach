/*
 * Copyright (c) 2018 Razeware LLC
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish,
 * distribute, sublicense, create a derivative work, and/or sell copies of the
 * Software in any work that is designed, intended, or marketed for pedagogical or
 * instructional purposes related to programming, coding, application development,
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works,
 * or sale is expressly withheld.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine;

public enum PieceType {King, Queen, Bishop, Knight, Rook, Pawn};

public abstract class Piece : MonoBehaviour
{
    public PieceType type;

    protected Vector2Int[] RookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    protected Vector2Int[] BishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};

    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);
    public bool moving = false;

    private Rigidbody body;
    private BoxCollider boxCollider;

    protected void Start()
    {
        GameObject piece = this.gameObject;

        body = piece.AddComponent<Rigidbody>();
        body.drag = 1;
        body.constraints = RigidbodyConstraints.FreezePositionY;

        boxCollider = piece.AddComponent<BoxCollider>();

        if (PieceType.King.Equals(type) || PieceType.Queen.Equals(type))
        {
            boxCollider.center = new Vector3(0, 1, 0);
            boxCollider.size = new Vector3(0.75f, 2, 0.75f);
        }
        else if(PieceType.Pawn.Equals(type))
        {
            boxCollider.center = new Vector3(0, 0.55f, 0);
            boxCollider.size = new Vector3(0.75f, 1, 0.75f);
        }
        else
        {
            boxCollider.center = new Vector3(0, 0.75f, 0);
            boxCollider.size = new Vector3(0.75f, 1.5f, 0.75f);
        }

        boxCollider.material = Resources.Load("ChessMaterial", typeof(PhysicMaterial)) as PhysicMaterial;
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject obstacle = collision.gameObject;
        GameManager gm = GameManager.instance;
        if ((!gm.DoesPieceBelongToCurrentPlayer(obstacle)) || obstacle.GetComponent<Piece>() == null)
        {
            Physics.IgnoreCollision(collision.collider, boxCollider);

            if(obstacle.GetComponent<Piece>() != null && !gm.GridForPiece(obstacle).Equals(gm.GridForPiece(this.gameObject)))
            {
                Rigidbody rb = obstacle.GetComponent<Piece>().GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else if (obstacle.GetComponent<Piece>() != null){
          this.moving = true;
        }
    }

    private void FixedUpdate(){
      GameManager gm = GameManager.instance;
      if (this.moving && gm.GridForPiece(this.gameObject).Equals(new Vector2Int(-1, -1)) && body.velocity.magnitude < 0.01 ){
          Destroy(this.gameObject);
      }
    }
}
