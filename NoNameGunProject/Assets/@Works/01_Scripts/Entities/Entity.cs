using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace NoNameGun.Entities
{
    // 엔티티 클래스: 모든 엔티티의 기본 클래스 역할을 합니다.
    // (선한쌤 코드)
    public class Entity : MonoBehaviour
    {
        // IEntityComponent를 저장할 딕셔너리
        // GetCompo 함수를 통해 접근할 수 있도록 설정
        protected Dictionary<Type, IEntityComponent> _components;

        // 객체 초기화 시 호출되는 Unity의 기본 메서드
        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>(); // 딕셔너리 초기화
            AddComponentToDictionary(); // 자식 컴포넌트들을 딕셔너리에 추가
            ComponentInit(); // 컴포넌트 초기화
            AfterInitalize(); // 초기화 이후 추가 작업
        }

        // 자식 객체에서 IEntityComponent를 찾아 딕셔너리에 추가
        private void AddComponentToDictionary()
        {
            GetComponentsInChildren<IEntityComponent>(true) // 자식 객체 포함, 모든 IEntityComponent 검색
                .ToList() // 결과를 리스트로 변환
                .ForEach(component => _components.Add(component.GetType(), component)); // 딕셔너리에 추가
        }

        // 딕셔너리에 저장된 모든 컴포넌트를 초기화
        private void ComponentInit()
        {
            _components.Values.ToList() // 모든 컴포넌트 가져오기
                .ForEach(component => component.Init(this)); // Init 메서드 호출
        }

        // 컴포넌트 초기화 이후 추가 작업 처리
        protected virtual void AfterInitalize()
        {
            _components.Values
                .OfType<IAfterInitable>() // IAfterInitable 인터페이스를 구현한 컴포넌트 필터링
                .ToList()
                .ForEach(afterInitCompo => afterInitCompo.AfterInIt()); // AfterInIt 메서드 호출
        }

        // 특정 타입의 컴포넌트를 반환하는 메서드
        public T GetCompo<T>(bool isDerived = false) where T : IEntityComponent
        {
            // 정확히 일치하는 타입의 컴포넌트가 있는지 확인
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
            {
                return (T)component; // 존재하면 캐스팅 후 반환
            }

            // isDerived가 false면 기본값 반환
            if (isDerived == false) return default;

            // 상속 관계를 고려하여 컴포넌트를 찾음
            Type findType = _components.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (findType != null)
            {
                return (T)_components[findType]; // 상속 관계 컴포넌트를 반환
            }

            // 해당 타입의 컴포넌트가 없으면 기본값 반환
            return default;
        }
    }
}
