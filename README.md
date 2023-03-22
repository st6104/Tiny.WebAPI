# Tiny.WebAPI
도메인주도개발(DDD) 방법론을 학습하기 위한 연습용 프로젝트

## 개발환경
Main Framework : .NET 7 (ASP.NET Web API)    
IDE : VS Code

## 프로젝트에 참조 중인 라이브러리  
| 라이브러리 | 설명 |
| - | - |
| Microsoft.EntityFrameworkCore<br>Microsoft.EntityFrameworkCore.SqlServer | SQL Server를 저장소로 쓰기 위한 ORM |
| [MediatR](https://github.com/jbogard/MediatR) | 대리자 패턴 라이브리러리<br>(CQRS구현 및 Validation Behavior, Transaction Behavior 구현에 사용) |
| [Ardalis.SmartEnum](https://github.com/ardalis/SmartEnum) | 도메인 엔티티 구현 시 더 강력한 형태의 Enum 구현 |
| [Ardalis.Result.AspNetCore<br>Ardalis.Result.FluentValidation](https://github.com/ardalis/Result) | Web API의 Request에 대한 Response를 일관된 형식으로 반환하기 위한 라이브러리<br>(40x 계열 응답에 대한 표준확립) |
| [FluentValidation.AspNetCore](https://github.com/FluentValidation/FluentValidation.AspNetCore) | 객체에 대한 유효성 검증에 사용<br>(MediatR의 Validation Behavior에서 사용) |
| [Scrutor](https://github.com/khellang/Scrutor) | 수많은 Repository / BusinessService 객체를 서비스로 일괄 등록하기 위한 라이브러리 |
| [Ardalis.Specification<br>Ardalis.Specification.EntityFrameworkCore](https://github.com/ardalis/Specification) | 명세(Specification) 패턴 구현을 간결하게 해줄 라이브러리 |

## 도입 예정인 라이브러리
| 라이브러리 | 설명 |
| - | - |
