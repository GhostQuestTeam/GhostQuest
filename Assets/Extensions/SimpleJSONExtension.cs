using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Extensions{

public static class SimpleJSONExtension {

		// TODO найти лучший способ реализовать это
		public static bool HasKey(this JSONNode node, string key){
			if (!node.IsObject) return false;
			var value = node [key];
			return (
				value.IsArray ||
				value.IsBoolean ||
				value.IsNull ||
				value.IsNumber ||
				value.IsObject ||
				value.IsString
			);
		}

}

}