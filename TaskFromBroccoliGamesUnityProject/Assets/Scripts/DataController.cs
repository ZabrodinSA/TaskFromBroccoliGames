using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ViewsComponent;


public class DataController : MonoBehaviour
{
    public float mapEdgeRight;
    public float mapEdgeLeft;
    public float mapEdgeTop;
    public float mapEdgeBot;

    private string path;
    private string jsonString;
    private float stageWidth = 0;
    private float stageHeight = 0;

    void Awake()
    {
        path = Application.dataPath + "/Resources/testing_views_settings.json";
        jsonString = File.ReadAllText(path);
        ListViewsSettings listViewsSettings = JsonUtility.FromJson<ListViewsSettings>(jsonString);
      
        for (int i=0; i<listViewsSettings.List.Count; i++)      
        {
            GameObject gameObject = new GameObject(listViewsSettings.List[i].Id, typeof(SpriteRenderer),typeof(ViewSettingsComponent));
            gameObject.tag = "Sprite";
            ViewSettingsComponent viewSettingsComponent = gameObject.GetComponent<ViewSettingsComponent>();
            listViewsSettings.List[i].SetViewSettingsComponent(viewSettingsComponent);
        }
        SetSprite();
    }

    private void SetSprite ()
    {   
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Sprite");
        ViewSettingsComponent[] viewSettingsComponents = new ViewSettingsComponent[gameObjects.Length];
        SpriteRenderer[] spriteRenderers = new SpriteRenderer[gameObjects.Length];
        for (int i=0; i<gameObjects.Length; i++)
        {
            viewSettingsComponents[i] = gameObjects[i].GetComponent<ViewSettingsComponent>();
            spriteRenderers[i] = gameObjects[i].GetComponent<SpriteRenderer>();
        }

        for (int i = 0; i < (gameObjects.Length); i++)                  //распологаем все спрайты по порядку по строчно
        {
              for (int j = 1; j < gameObjects.Length - i; j++)
              {
                    if ((viewSettingsComponents[i + j].Y > viewSettingsComponents[i].Y) ||
                            (viewSettingsComponents[i+j].Y==viewSettingsComponents[i].Y && viewSettingsComponents[i+j].X<viewSettingsComponents[i].X))
                    {
                        ViewSettingsComponent temp = new ViewSettingsComponent();
                        temp.Set(viewSettingsComponents[i]);
                        viewSettingsComponents[i].Set(viewSettingsComponents[i + j]);
                        viewSettingsComponents[i + j].Set(temp);
                        gameObjects[i].name = viewSettingsComponents[i].Id;
                        gameObjects[i+j].name = viewSettingsComponents[i+j].Id;
                    }
              }
              spriteRenderers[i].sprite = Resources.Load<Sprite>(viewSettingsComponents[i].Id);
        }
                                 //размещаем спрайты вплотную друг к другу и вычисляем размеры сцены для ограничения перемещения камеры
        gameObjects[0].transform.position = new Vector3(viewSettingsComponents[0].X, viewSettingsComponents[0].Y, 0);
        stageWidth += viewSettingsComponents[0].Width;
        stageHeight += viewSettingsComponents[0].Height;
        bool firstLineFlag = true;
        for (int i=1; i<gameObjects.Length; i++)                
        {
            if (viewSettingsComponents[i].Y == viewSettingsComponents[i-1].Y)
            {
                gameObjects[i].transform.position = new Vector3(gameObjects[i-1].transform.position.x+(viewSettingsComponents[i].Width+viewSettingsComponents[i-1].Width)/2,
                                                                 viewSettingsComponents[i].Y, 0);
                if (firstLineFlag)
                {
                    stageWidth += viewSettingsComponents[i].Width;
                }
            } else
            {
                gameObjects[i].transform.position = new Vector3(viewSettingsComponents[i].X, 
                                        gameObjects[i-1].transform.position.y-(viewSettingsComponents[i].Height + viewSettingsComponents[i - 1].Height) / 2, 0);
                stageHeight += viewSettingsComponents[i].Height;
                firstLineFlag = false;
            }
        }

        mapEdgeRight = viewSettingsComponents[0].X + stageWidth - viewSettingsComponents[0].Width/2;
        mapEdgeLeft = viewSettingsComponents[0].X - viewSettingsComponents[0].Height/2;
        mapEdgeTop = viewSettingsComponents[0].Y + viewSettingsComponents[0].Height/2;
        mapEdgeBot = viewSettingsComponents[0].Y - stageHeight + viewSettingsComponents[0].Height/2;
}

    [System.Serializable]
    public class ViewsSetting
    {
        public string Id;
        public string Type;
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public void SetViewSettingsComponent(ViewSettingsComponent viewSettingsComponent)
        {
            viewSettingsComponent.Id = Id;
            viewSettingsComponent.Type = Type;
            viewSettingsComponent.X = X;
            viewSettingsComponent.Y = Y;
            viewSettingsComponent.Width = Width;
            viewSettingsComponent.Height = Height;
        }
    }
   
    [System.Serializable]
    public class ListViewsSettings
    {
        public List<ViewsSetting> List;
    }
}
