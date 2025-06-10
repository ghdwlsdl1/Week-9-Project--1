# ATM 시스템 과제

ATM 시스템을 Unity 기반으로 구현한 프로젝트입니다. 유저의 입출금, 송금, 로그인 및 회원가입 기능을 포함한 UI 기반 은행 기능을 구현했습니다.

---

## 주요 기능

| 기능 구분 | 설명 |
|-----------|------|
| 입금 기능 | 1만원, 3만원, 5만원 고정 버튼 및 사용자 입력 기반 입금 |
| 출금 기능 | 잔액 조건 검사 포함, UI 및 로직 구현 완료 |
| 로그인 / 회원가입 | JSON 기반 사용자 데이터 관리, ID 중복 확인 및 비밀번호 검증 포함 |
| 송금 기능 | 다른 사용자에게 금액 송금, 존재 여부 확인 및 잔액 조건 처리 |
| 예외 처리 | 공백 입력, 금액 오류, ID 없음, 비밀번호 불일치, 숫자 아님 등 다양한 조건 처리 |
| 데이터 저장/로드 | Application.persistentDataPath 기준 JSON 저장 방식 |
| UI 관리 | 입출금, 송금, 에러 팝업 등 모든 화면 상태 전환 처리 완료 |

---

## 파일 구조

```
Assets/
├── Scripts/
│   ├── PopupBank.cs
│   ├── PopupLogin.cs
│   ├── PopupSignUp.cs
│   ├── GameManager.cs
│   └── UserData.cs
```

---

## 데이터 저장 구조

- 사용자 데이터는 `Application.persistentDataPath` 경로에 `ID.json` 파일로 저장됩니다.
- 저장 포맷 예시:

```json
{
  "id": "hongjin",
  "password": "1234",
  "userName": "최홍진",
  "balance": 100000,
  "cash": 50000
}
```

---
