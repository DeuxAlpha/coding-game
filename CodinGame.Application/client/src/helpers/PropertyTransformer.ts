export function toCamel(theObject: any): any {
  if (!theObject) return theObject;
  let newObject: any = {}
  if (theObject instanceof Array) {
    return theObject.map(function (value) {
      if (typeof value === "object") {
        value = toCamel(value)
      }
      return value
    })
  } else {
    for (let originalKey in theObject) {
      if (theObject.hasOwnProperty(originalKey)) {
        let newKey = (originalKey.charAt(0).toLowerCase() + originalKey.slice(1) || originalKey).toString()
        let value = theObject[originalKey]
        if (value instanceof Array || (value !== null && value.constructor === Object)) {
          value = toCamel(value)
        }
        newObject[newKey] = value
      }
    }
  }
  return newObject
}

export function toPascal(theObject: any): any {
  if (!theObject) return theObject;
  let newObject: any = {}
  if (theObject instanceof Array) {
    return theObject.map(function (value) {
      if (typeof value === "object") {
        value = toPascal(value)
      }
      return value
    })
  } else {
    for (let originalKey in theObject) {
      if (theObject.hasOwnProperty(originalKey)) {
        let newKey = (originalKey.charAt(0).toUpperCase() + originalKey.slice(1) || originalKey).toString()
        let value = theObject[originalKey]
        if (value instanceof Array || (value !== null && value.constructor === Object)) {
          value = toPascal(value)
        }
        newObject[newKey] = value
      }
    }
  }
  return newObject
}
