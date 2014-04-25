using System.ServiceModel;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;

namespace Jason.Model
{
	public abstract class JobTask : AbstractDataObject
	{
		public String CorrelationId { get; set; }

		protected JobTask()
		{
			this.CorrelationId = Guid.NewGuid().ToString();
		}

//#if !SILVERLIGHT

//		private IDictionary<String, Object> bag = new Dictionary<string, object>();

//		public void SetData( String key, Object value )
//		{
//			this.bag.Add( key, value );
//		}

//		public Boolean TryGetData<T>( String key, out T data )
//		{
//			if( this.bag.ContainsKey( key ) )
//			{
//				var obj = this.bag[ key ];
//				if( obj is T )
//				{
//					data = ( T )obj;
//					return true;
//				}
//			}

//			data = default( T );
//			return false;
//		}

//#endif
	}
}
