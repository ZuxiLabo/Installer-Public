using HVL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Tiny {

	using Decoder = Func<Type, object, object>;

	public static class DefaultDecoder {

		public static Decoder GenericDecoder() {
			return (type, jsonObj) => {
				object instance = Activator.CreateInstance(type, true);
				if (jsonObj is IDictionary) {
					foreach (DictionaryEntry item in (IDictionary)jsonObj) {
						string name = (string)item.Key;
						if (!JsonMapper.DecodeValue(instance, name, item.Value)) {
							Console.WriteLine("Couldn't decode field \"" + name + "\" of " + type);
						}
					}
				} else {
					Console.WriteLine("Unsupported json type: " + (jsonObj != null ? jsonObj.GetType().ToString() : "null"));
				}
				return instance;
			}; 
		}

		public static Decoder DictionaryDecoder() {
			return (type, jsonObj) => {

				Program.Print("Decode Dictionary",Program.IsDebug);

				// Dictionary
				if (jsonObj is IDictionary<string, object>) {
					Dictionary<string, object> jsonDict = (Dictionary<string, object>)jsonObj;
					if (type.GetGenericArguments().Length == 2) {
						IDictionary instance = null;
						Type keyType = type.GetGenericArguments()[0];
						Type genericType = type.GetGenericArguments()[1];
						bool nullable = genericType.IsNullable();
						if (type != typeof(IDictionary) && typeof(IDictionary).IsAssignableFrom(type)) {
#if DEBUG
							Console.WriteLine("Create Dictionary Instance");
#endif
							instance = Activator.CreateInstance(type, true) as IDictionary;
						} else {
#if DEBUG
							Console.WriteLine("Create Dictionary Instance for IDictionary interface");
#endif
							Type genericDictType = typeof(Dictionary<,>).MakeGenericType(keyType, genericType);
							instance = Activator.CreateInstance(genericDictType) as IDictionary;
						}
						foreach (KeyValuePair<string, object> item in jsonDict) {
#if DEBUG
							Console.WriteLine(item.Key + " = " + JsonMapper.DecodeValue(item.Value, genericType));
#endif
							object value = JsonMapper.DecodeValue(item.Value, genericType);
							object key = item.Key;
							if (keyType == typeof(int)) key = Int32.Parse(item.Key);
							if (value != null || nullable) instance.Add(key, value);
						}
						return instance;
					} else {


						Program.Print("Unexpected type arguemtns", Program.IsDebug);

					}
				}
				// Dictionary (convert int to string key)
				if (jsonObj is IDictionary<int, object>) {
					Dictionary<string, object> jsonDict = new Dictionary<string, object>();
					foreach (KeyValuePair<int, object> keyValuePair in (Dictionary<int, object>)jsonObj) {
						jsonDict.Add(keyValuePair.Key.ToString(), keyValuePair.Value);
					}
					if (type.GetGenericArguments().Length == 2) {
						IDictionary instance = null;
						Type keyType = type.GetGenericArguments()[0];
						Type genericType = type.GetGenericArguments()[1];
						bool nullable = genericType.IsNullable();
						if (type != typeof(IDictionary) && typeof(IDictionary).IsAssignableFrom(type)) {
							instance = Activator.CreateInstance(type, true) as IDictionary;
						} else {
							Type genericDictType = typeof(Dictionary<,>).MakeGenericType(keyType, genericType);
							instance = Activator.CreateInstance(genericDictType) as IDictionary;
						}
						foreach (KeyValuePair<string, object> item in jsonDict) {
							Console.WriteLine(item.Key + " = " + JsonMapper.DecodeValue(item.Value, genericType));
							object value = JsonMapper.DecodeValue(item.Value, genericType);
							if (value != null || nullable) instance.Add(Convert.ToInt32(item.Key), value);
						}
						return instance;
					} else {
						Console.WriteLine("Unexpected type arguemtns");
					}
				}

				Program.Print("Couldn't decode Dictionary: " + type, Program.IsDebug);

				return null;
			};
		}

		public static Decoder ArrayDecoder() {
			return (type, jsonObj) => {
				if (typeof(IEnumerable).IsAssignableFrom(type)) {
					if (jsonObj is IList) {
						IList jsonList = (IList)jsonObj;
						if (type.IsArray) {
							Type elementType = type.GetElementType();
							bool nullable = elementType.IsNullable();
							var array = Array.CreateInstance(elementType, jsonList.Count);
							for (int i = 0; i < jsonList.Count; i++) {
								object value = JsonMapper.DecodeValue(jsonList[i], elementType);
								if (value != null || nullable) array.SetValue(value, i);
							}
							return array;
						}
					}
				}

                Program.Print("Couldn't decode Array: " + type, Program.IsDebug);

				return null;
			};
		}

		public static Decoder ListDecoder() {
			return (type, jsonObj) => {
				if (type.HasGenericInterface(typeof(IList<>)) && type.GetGenericArguments().Length == 1) {
					Type genericType = type.GetGenericArguments()[0];
					if (jsonObj is IList) {
						IList jsonList = (IList)jsonObj;
						IList instance = null;
						bool nullable = genericType.IsNullable();
						if (type != typeof(IList) && typeof(IList).IsAssignableFrom(type)) {
							instance = Activator.CreateInstance(type, true) as IList;
						} else {
							Type genericListType = typeof(List<>).MakeGenericType(genericType);
							instance = Activator.CreateInstance(genericListType) as IList;
						}
						foreach (var item in jsonList) {
							object value = JsonMapper.DecodeValue(item, genericType);
							if (value != null || nullable) instance.Add(value);
						}
						return instance;
					}
				}

                Program.Print("Couldn't decode List: " + type, Program.IsDebug);

				return null;
			};
		}

		public static Decoder CollectionDecoder() {
			return (type, jsonObj) => {
				if (type.HasGenericInterface(typeof(ICollection<>))) {
					Type genericType = type.GetGenericArguments()[0];
					if (jsonObj is IList) {
						IList jsonList = (IList)jsonObj;
						var listType = type.IsInstanceOfGenericType(typeof(HashSet<>)) ? typeof(HashSet<>) : typeof(List<>);
						var constructedListType = listType.MakeGenericType(genericType);
						var instance = Activator.CreateInstance(constructedListType, true);
						bool nullable = genericType.IsNullable();
						MethodInfo addMethodInfo = type.GetMethod("Add");
						if (addMethodInfo != null) {
							foreach (var item in jsonList) {
								object value = JsonMapper.DecodeValue(item, genericType);
								if (value != null || nullable) addMethodInfo.Invoke(instance, new object[] { value });
							}
							return instance;
						}
					}
				}
                Program.Print("Couldn't decode Collection: " + type, Program.IsDebug);
				return null;
			};
		}

		public static Decoder EnumerableDecoder() {
			return (type, jsonObj) => {
				if (typeof(IEnumerable).IsAssignableFrom(type)) {
					// It could be an dictionary
					if (jsonObj is IDictionary) {
						// Decode a dictionary
						return DictionaryDecoder().Invoke(type, jsonObj);
					}

					// Or it could be also be a list
					if (jsonObj is IList) {
						// Decode an array
						if (type.IsArray) {
							return ArrayDecoder().Invoke(type, jsonObj);
						} 

						// Decode a list
						if (type.HasGenericInterface(typeof(IList<>))) {
							return ListDecoder().Invoke(type, jsonObj);
						} 

						// Decode a collection
						if (type.HasGenericInterface(typeof(ICollection<>))) {
							return CollectionDecoder().Invoke(type, jsonObj);
						} 
					}
				}

                Program.Print("Couldn't decode Enumerable: " + type, Program.IsDebug);
				return null;
			};
		}
	}
}
