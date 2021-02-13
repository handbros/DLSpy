using System;
using System.Collections.Generic;

namespace DLSpy.Entities
{
    /// <summary>
    /// 작품 정보를 저장하는 클래스입니다.
    /// </summary>
    public class WorkInformation : IDisposable
    {
        public string Code { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Title { get; set; }

        public string Circle { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; }

        public float Score { get; set; }

        public int Stars { get; set; }

        public int DownloadCount { get; set; }

        public int Price { get; set; }

        public string PriceString { get; set; }

        public string FullInformation { get; set; }

        public WorkInformation()
        {
            Tags = new List<string>();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                    Code = null;
                    ThumbnailUrl = null;
                    Title = null;
                    Circle = null;
                    Description = null;
                    Tags.Clear();
                    Tags = null;
                    PriceString = null;
                    FullInformation = null;
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~WorkInformation()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
