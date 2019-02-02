using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Object = UnityEngine.Object;


[CustomPropertyDrawer(typeof(ShopReadyProfile))]
public class ShopReadyProfileDrawer : PropertyDrawer
{


	private Rect _rect;



	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return 64.0f;
	}


	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{
		_rect = rect;

		GUIStyle style = GUI.skin.label;

		Rect rowRect = new Rect(rect)
		{
			height = 16.0f,
			y = rect.y + rect.height * 0.5f - 9.0f
		};

		if (property == null)
		{
			return;
		}

		CylinderProfile cylinderProfile = (CylinderProfile) property.objectReferenceValue;

		if (cylinderProfile == null)
		{
			return;
		}

		SerializedObject so = new SerializedObject(cylinderProfile);
		so.Update();

		float margin = 0.03f;

		// Thumb
		Rect imageRect = new Rect(rowRect)
		{
			height = 64.0f,
			width = 64.0f,
			y = _rect.y,
			x = _rect.x + _rect.width - 64.0f
		};
		Object image = so.FindProperty("_thumbnail").objectReferenceValue;
		if (image != null)
		{
			EditorGUI.DrawTextureTransparent(imageRect, AssetPreview.GetAssetPreview(image));
		}
		rowRect.width -= 70.0f;
		
		// Ordering Num
		Rect numRect = rowRect.CalcPlace(0.03f, margin, out rowRect);
		EditorGUI.LabelField(numRect, label);

		// SO
		Rect assetRect = rowRect.CalcPlace(0.4f, margin, out rowRect);
		EditorGUI.ObjectField(assetRect, property, GUIContent.none);

		// Material
		Rect matRect = rowRect.CalcPlace(0.6f, margin, out rowRect);
		EditorGUI.ObjectField(matRect, so.FindProperty("_skin"), GUIContent.none);
		
		// Cost
		Rect costRect = rowRect.CalcPlace(1.0f, margin, out rowRect);
		label.text = "$:";
		EditorGUIUtility.labelWidth = style.CalcSize(label).x;
		EditorGUI.PropertyField(costRect, so.FindProperty("_cost"), label);

		so.ApplyModifiedProperties();
	}
	

}


public static class RectExt
{


	private const bool _IS_DEBUG = false;
	private static readonly Color _DEB_COLOR = new Color(0, 1, 1, 0.2f);


	public static Rect CalcPlace(this Rect r1, float percent, float margin, out Rect r1R)
	{
		float width = r1.width * (percent + margin);
		r1R = r1;
		r1R.width -= width;
		r1R.x += width;

		r1.width *= percent;

		if (_IS_DEBUG)
		{
			EditorGUI.DrawRect(r1, _DEB_COLOR);
		}

		return r1;
	}


	public static Rect CalcPlaceFixed(this Rect r1, float fixedWidth, float margin, out Rect r1R)
	{
		float width = fixedWidth + margin;
		r1R = r1;
		r1R.width -= width;
		r1R.x += width;

		r1.width = fixedWidth;

		if (_IS_DEBUG)
		{
			EditorGUI.DrawRect(r1, _DEB_COLOR);
		}

		return r1;
	}

	
}


public class CylinderStylesWindow : ShopReadyItemsWindow
{


	[MenuItem("Emi Stack/Cylinder Styles Special #&c")]
	private static CylinderStylesWindow Init()
	{
		CylinderStylesWindow window = GetWindow<CylinderStylesWindow>();
		window.titleContent = new GUIContent("Cylinder Styles");
		window.Focus();
		window.Repaint();
		return window;
	}


	private void OnGUI()
	{
		base.OnGUI<CylinderProfile>();
	}


}



public class ShopReadyItemsWindow : EditorWindow
{


	private string _styleName;
	private Sprite _thumbnail;
	private Material _baseMaterial;
	private Texture2D _emissionTexture;
	private int _cost;

	private ShopReadyProfileSet _shopReadySet;
	private SerializedObject _shopSetSerializedObject;
	private SerializedProperty _shopItemsSerializedProperty;
	private ReorderableList _rList;
	private int _activeItem = -1;
	private Vector2 _scrollPos;


	
	private void OnEnable()
	{
		string time = DateTime.Now.ToString("dd-MM-yy");
		_styleName = $"New profile {time}";

		Selection.selectionChanged += CheckSelection;
		InitReorderableList();
	}

	
	private void CheckSelection()
	{
		if (Selection.activeObject is ShopReadyProfileSet)
		{
			ShopReadyProfileSet srpSet = (ShopReadyProfileSet) Selection.activeObject;

			if (srpSet != null)
			{
				_shopReadySet = srpSet;
				InitReorderableList();
				this.Repaint();
				return;
			}
		}
		_shopReadySet = null;
		this.Repaint();
	}


	private void InitReorderableList()
	{
		if (_shopReadySet == null)
		{
			return;
		}

		_shopSetSerializedObject = new SerializedObject(_shopReadySet);
		_shopItemsSerializedProperty = _shopSetSerializedObject.FindProperty("items");
		_rList = new ReorderableList
		(
			_shopSetSerializedObject, 
			_shopItemsSerializedProperty, 
			true, 
			false, 
			false, 
			true
		);

		_rList.drawHeaderCallback += DrawHeader;
		_rList.drawElementCallback += DrawElement;
		_rList.onRemoveCallback += RemoveItem;
		_rList.onMouseUpCallback += SelectItem;
		_rList.drawElementBackgroundCallback += Bck;
		//_rList.showDefaultBackground += DefaultBck;
		_rList.elementHeight = 64.0f;
	}


	private void Bck(Rect rect, int index, bool isactive, bool isfocused)
	{
		Color c = Color.cyan;
		if (isactive || isfocused)
		{
			c = Color.red;
		}

		c.a = 0.1f;
		EditorGUI.DrawRect(rect, c);
	}


	private void SelectItem(ReorderableList list)
	{
		SerializedObject srp;
		SerializedProperty cyl = _shopItemsSerializedProperty.GetArrayElementAtIndex(_activeItem);

		if (cyl.objectReferenceValue != null)
		{
			srp = new SerializedObject(cyl.objectReferenceValue);
			string path = AssetDatabase.GetAssetPath(cyl.objectReferenceValue);
			_styleName = Path.GetFileNameWithoutExtension(path);
			_cost = srp.FindProperty("_cost").intValue;
			_baseMaterial = (Material)srp.FindProperty("_skin").objectReferenceValue;
			_emissionTexture = (Texture2D)_baseMaterial.GetTexture("_EmissionMap");
			_thumbnail = (Sprite)srp.FindProperty("_thumbnail").objectReferenceValue;
		}
	}


	private void RemoveItem(ReorderableList list)
	{
		SerializedProperty sp = _shopItemsSerializedProperty.GetArrayElementAtIndex(_activeItem);
		ShopReadyProfile srp = (ShopReadyProfile) sp.objectReferenceValue;
		srp?.OnDestroy();
		_shopReadySet?.Remove(srp);

		string assetPath = AssetDatabase.GetAssetPath(srp);
		AssetDatabase.DeleteAsset(assetPath);
	}


	private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
	{
		SerializedProperty cyl = _shopItemsSerializedProperty.GetArrayElementAtIndex(index);
		EditorGUI.PropertyField(rect, cyl, new GUIContent(index.ToString()));

		float separatorHeight = 2.0f;
		Rect separatorRect = new Rect(rect)
		{
			height = separatorHeight * 0.5f,
			y = rect.yMax - separatorHeight
		};

		if (index < _shopItemsSerializedProperty.arraySize - 1)
		{
			EditorGUI.DrawRect(separatorRect, Color.gray);
			separatorRect.y += separatorRect.height;
			EditorGUI.DrawRect(separatorRect, Color.black);
		}

		if (isActive)
		{
			_activeItem = index;
		}

		//if (isFocused)
		//{
		//	Debug.Log($"{index} is Focused");
		//}
	}


	private void DrawHeader(Rect rect)
	{
		GUI.Label(rect, $"{_rList.count} / {_shopItemsSerializedProperty.arraySize} Items");
	}


	protected void OnGUI<T>() where T : ShopReadyProfile
	{
		EditorGUILayout.Space();

		_styleName = EditorGUILayout.TextField("Name:", _styleName);
		_cost = EditorGUILayout.IntField("Cost: ", _cost);
		_thumbnail = (Sprite)EditorGUILayout.ObjectField("Thumbnail: ", _thumbnail, typeof(Sprite), false);
		_baseMaterial = (Material)EditorGUILayout.ObjectField("Base material: ", _baseMaterial, typeof(Material), false);
		_emissionTexture = (Texture2D)EditorGUILayout.ObjectField("Emission texture: ", _emissionTexture, typeof(Texture2D), false);

		// List
		//EditorGUI.BeginChangeCheck();
		//{
		//	_shopReadySet = (ShopReadyProfileSet)EditorGUILayout.ObjectField("Shop Ready Items: ", _shopReadySet, typeof(ShopReadyProfileSet), false);
		//}
		//if (EditorGUI.EndChangeCheck())
		//{
		//	InitReorderableList();
		//}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		Color def = GUI.backgroundColor;
		GUI.backgroundColor = new Color(1.0f, 0.65f, 0.45f, 1.0f);
		if (GUILayout.Button("Save Changes") && _baseMaterial != null)
		{
			string path = $"Assets/Profiles/Cylinders/{_styleName}";
			Object cp = AssetDatabase.LoadAssetAtPath<Object>($"{path}.asset");

			if (cp != null)
			{
				_baseMaterial.SetTexture("_EmissionMap", _emissionTexture);
				EditSerializedObject(cp, _cost, _baseMaterial, _thumbnail);
			}
			Debug.Log(path);
		}
		GUI.backgroundColor = new Color(0.45f, 0.65f, 1.0f, 1.0f);
		if (GUILayout.Button("Create New") && _baseMaterial != null)
		{
			string path = $"Assets/Materials/Cylinder skins/{_styleName}";

			Material material = new Material(_baseMaterial);
			material.SetTexture("_EmissionMap", _emissionTexture);
			AssetDatabase.CreateAsset(material, $"{path}.mat");

			path = $"Assets/Profiles/Cylinders/{_styleName}";
			T cp = CreateInstance<T>();
			EditSerializedObject(cp, _cost, material, _thumbnail);
			AssetDatabase.CreateAsset(cp, $"{path}.asset");

			if (_shopReadySet != null)
			{
				_shopReadySet.Add(cp);
			}
			
			Debug.Log(path);
		}

		if (_shopReadySet == null || _rList == null)
		{
			return;
		}
		GUI.backgroundColor = def;
		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
		{
			_shopSetSerializedObject.Update();
			_rList.DoLayoutList();
			_shopSetSerializedObject.ApplyModifiedProperties();
		}
		EditorGUILayout.EndScrollView();
	}


	private void EditSerializedObject(Object obj, int cost, Material mat, Sprite thumb)
	{
		SerializedObject so = new SerializedObject(obj);
		so.FindProperty("_cost").intValue = cost;
		so.FindProperty("_skin").objectReferenceValue = mat;
		so.FindProperty("_thumbnail").objectReferenceValue = thumb;
		so.ApplyModifiedProperties();
	}


}
