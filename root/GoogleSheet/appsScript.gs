var sheetId = SpreadsheetApp.openById("16feKB5iMB7abHiWgz5pFegdbceKS_wVCEdRxVfjJNGc");
var userSheet = sheetId.getSheets()[1]; // 유저 목록 시트
var loginSheet = sheetId.getSheets()[2]; // 로그인 로그 시트
var playSheet = sheetId.getSheets()[3]; // 플레이 로그 시트
var cache = CacheService.getScriptCache();
var p;
var prof;
var result, msg, value;

function removeCache() {
  cache.removeAll(["uid", "row", "loginTime"]);
}

function response() {
  var json = {};
  json.order = p.order;
  json.result = result;
  json.msg = msg;
  json.value = value || ""; // value가 undefined일 경우 빈 문자열로 설정

  var jsonData = JSON.stringify(json);
  return ContentService.createTextOutput(jsonData).setMimeType(ContentService.MimeType.JSON);
}

function doPost(e) {
  p = e.parameter;

  switch (p.order) {
    case "register": removeCache(); register(); break;
    case "login": removeCache(); login(); break;
    case "logout": logout(); break;
    case "log": log(p); break;
  }

  return response();
}

function log(p)
{
  playSheet.appendRow([p.id, p.type, new Date(), p.description]);
}

function doGet(e) {
  const response = {
    status: 'success',
    data: '스프레드 시트 연동'
  };
  return ContentService.createTextOutput(JSON.stringify(response)).setMimeType(ContentService.MimeType.JSON);
}

function setResult(_result, _msg) {
  result = _result;
  msg = _msg;
}

function logAction(userId, action, date) {
  let year = date.getFullYear();
  let month = ('0' + (date.getMonth() + 1)).slice(-2); // 월은 0부터 시작하므로 +1
  let day = ('0' + date.getDate()).slice(-2);
  let hours = ('0' + date.getHours()).slice(-2);
  let minutes = ('0' + date.getMinutes()).slice(-2);
  let seconds = ('0' + date.getSeconds()).slice(-2);
  let formattedDate = year + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + seconds;

  loginSheet.appendRow([userId, action, formattedDate]);
}

function login() {
  if (!getProfile()) {
    setResult("ERROR", "Login failed. Please check your ID and password.");
    return;
  }

  var cell = userSheet.getRange(cache.get("row"), 4).getValue();

  var userId = cache.get("uid");
  var userName = cell;
  setResult("OK", userId + ":" + userName);
}

function getProfile() {
  var cell = userSheet.getRange(2, 1, userSheet.getLastRow() - 1, 3).getValues(); // uID, ID, Password 범위 가져오기

  for (var i = 0; i < cell.length; i++) {
    if (cell[i][1] != p.id || cell[i][2] != p.pass) // ID와 Password 비교
      continue;

    cache.put("uid", cell[i][0].toString()); 
    cache.put("row", (i + 2).toString()); 

    var uIDthis = cell[i][0].toString();
    prof = i;
    todaylogin = new Date();
    cache.put("loginTime", todaylogin.toString()); // 로그인 시간을 캐시에 저장

    let yearlogin = todaylogin.getFullYear();
    let monthlogin = ('0' + (todaylogin.getMonth() + 1)).slice(-2); // 월은 0부터 시작하므로 +1
    let datelogin = ('0' + todaylogin.getDate()).slice(-2);
    let hourslogin = ('0' + todaylogin.getHours()).slice(-2);
    let minuteslogin = ('0' + todaylogin.getMinutes()).slice(-2);
    let secondslogin = ('0' + todaylogin.getSeconds()).slice(-2);
    let formattedLoginDate = yearlogin + "-" + monthlogin + "-" + datelogin + " " + hourslogin + ":" + minuteslogin + ":" + secondslogin;

    userSheet.getRange(i + 2, 6).setValue(formattedLoginDate);

    logAction(uIDthis, "login", todaylogin);
    return true;
  }
  return false;
}

function logout() {
  var loginTimeStr = cache.get("loginTime");
  if (!loginTimeStr) {
    setResult("ERROR", "Login time not set.");
    return;
  }

  // 캐시에서 uid 값을 가져와서 로그 기록
  var userId = cache.get("uid");
  if (userId) {
    logAction(userId, "logout", logoutDate);
  } else {
    
    setResult("ERROR", "Failed to get UID from cache.");
    return;
  }

  setResult("OK", "logout");
  removeCache();
}

function register() {
  var cell = userSheet.getRange(2, 2, userSheet.getLastRow() - 1, 1).getValues(); // ID만 가져오기

  if (cell.some(row => row[0] == p.id)) { // 아이디 중복 체크
    setResult("ERROR", "이미 존재하는 아이디입니다.");
    return;
  }

  // uID 값을 설정하는 부분 수정
  var lastRow = userSheet.getLastRow();
  var uID;
  var lastUIDCell = userSheet.getRange(lastRow, 1).getValue();
  if (lastUIDCell) {
    uID = parseInt(lastUIDCell) + 1; // 마지막 uID 값을 읽어서 1 증가
  } else {
    uID = 1; // 시트가 비어있으면 uID를 1로 설정
  }
  
  let today = new Date();
  let year = today.getFullYear();
  let month = ('0' + (today.getMonth() + 1)).slice(-2); // 월은 0부터 시작하므로 +1
  let date = ('0' + today.getDate()).slice(-2);
  let hours = ('0' + today.getHours()).slice(-2);
  let minutes = ('0' + today.getMinutes()).slice(-2);
  let seconds = ('0' + today.getSeconds()).slice(-2);
  
  userSheet.appendRow([uID, p.id, p.pass, p.nickname, year + "-" + month + "-" + date + " " + hours + ":" + minutes + ":" + seconds]);

  logAction(uID, "register", today);
  setResult("OK", "Register complete");
}

