#pragma once

#include <set>
#include <list>
#include <deque>
#include <vector>
#include <iterator>
#include <limits.h>
#include <assert.h>
#include <functional>
#include <allocators>
#include <type_traits>

namespace boolinq
{
    // all(xxx) and all(xxx,lambda)

    template<typename R, typename F>
    bool all(R r, F f)
    {
        while (!r.empty())
            if (!f(r.popFront()))
                return false;
        return true;
    }

    template<typename R>
    bool all(R r)
    {
        typedef typename R::value_type value_type;
        return all(r,[](const value_type & a)->value_type{return a;});
    }

    // xxx.all() and xxx.all(lambda)

    template<template<typename> class TLinq, typename R>
    class All_mixin
    {
    public:
        bool all() const
        {
            return boolinq::all(((TLinq<R>*)this)->r);
        }

        template<typename F>
        bool all(F f) const
        {
            return boolinq::all(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    // any(xxx) and any(xxx,lambda)

    template<typename R, typename F>
    bool any(R r, F f)
    {
        while (!r.empty())
            if (f(r.popFront()))
                return true;
        return false;
    }

    template<typename R>
    bool any(R r)
    {
        typedef typename R::value_type value_type;
        return any(r,[](const value_type & a)->value_type{return a;});
    }

    // xxx.any() and xxx.any(lambda)

    template<template<typename> class TLinq, typename R>
    class Any_mixin
    {
    public:
        bool any() const
        {
            return boolinq::any(((TLinq<R>*)this)->r);
        }

        template<typename F>
        bool any(F f) const
        {
            return boolinq::any(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    // avg(xxx) and avg(xxx,lambda)

    template<typename R, typename F>
    auto avg(R r, F f) -> decltype(f(r.front()) + f(r.back()))
    {
        typedef decltype(f(r.front()) + f(r.back())) value_type;
        value_type val = value_type();
        int count = 0;
        for (; !r.empty(); r.popFront())
        {
            val = val + f(r.front());
            count++;
        }
        return val/count;
    }

    template<typename R>
    auto avg(R r) -> decltype(r.front() + r.back())
    {
        typedef decltype(r.front() + r.back()) value_type;
        return avg(r,[](const value_type & a)->value_type{return a;});
    }

    // xxx.avg() and xxx.avg(lambda)

    template<template<typename> class TLinq, typename R>
    class Avg_mixin
    {
        template<typename F, typename TArg>
        static auto get_return_type(F * f = NULL, TArg * arg = NULL)
            -> decltype((*f)(*arg));          

    public:
        //TODO: Invalid return type ... should be (value_type + value_type)
        typename R::value_type avg() const
        {
            return boolinq::avg(((TLinq<R>*)this)->r);
        }

        template<typename F>
        auto avg(F f) const -> decltype(get_return_type<F,typename R::value_type>()
                                        + get_return_type<F,typename R::value_type>())
        {
            return boolinq::avg(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    // sum(xxx) and sum(xxx,lambda)

    template<typename R, typename F>
    auto sum(R r, F f) -> decltype(f(r.front()) + f(r.back()))
    {
        typedef decltype(f(r.front()) + f(r.back())) value_type;
        value_type val = value_type();
        for (; !r.empty(); r.popFront())
            val = val + f(r.front());
        return val;
    }

    template<typename R>
    auto sum(R r) -> decltype(r.front() + r.back())
    {
        typedef decltype(r.front() + r.back()) value_type;
        return sum(r,[](const value_type & a)->value_type{return a;});
    }

    // xxx.sum() and xxx.sum(lambda)

    template<template<typename> class TLinq, typename R>
    class Sum_mixin
    {
        template<typename F, typename TArg>
        static auto get_return_type(F * f = NULL, TArg * arg = NULL)
                    -> decltype((*f)(*arg));          

    public:
        //TODO: Invalid return type ... should be (value_type + value_type)
        typename R::value_type sum() const
        {
            return boolinq::sum(((TLinq<R>*)this)->r);
        }

        template<typename F>
        auto sum(F f) const -> decltype(get_return_type<F,typename R::value_type>()
                                        + get_return_type<F,typename R::value_type>())
        {
            return boolinq::sum(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    // min(xxx, int)

    template<typename R, typename F>
    typename R::value_type min(R r, F f)
    {
        R min = r;
        while (!r.empty())
        {
            if (f(r.front()) < f(min.front()))
                min = r;
            r.popFront();
        }
        return min.front();
    }

    template<typename R>
    typename R::value_type min(R r)
    {
        typedef typename R::value_type value_type;
        return min(r,[](const value_type & a)->value_type{return a;});
    }

    // xxx.min(int)

    template<template<typename> class TLinq, typename R>
    class Min_mixin
    {
    public:
        typename R::value_type min() const
        {
            return boolinq::min(((TLinq<R>*)this)->r);
        }

        template<typename F>
        typename R::value_type min(F f) const
        {
            return boolinq::min(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    // max(xxx, int)

    template<typename R, typename F>
    typename R::value_type max(R r, F f)
    {
        R max = r;
        while (!r.empty())
        {
            if (f(r.front()) > f(max.front()))
                max = r;
            r.popFront();
        }
        return max.front();
    }

    template<typename R>
    typename R::value_type max(R r)
    {
        typedef typename R::value_type value_type;
        return max(r,[](const value_type & a)->value_type{return a;});
    }

    // xxx.max(int)

    template<template<typename> class TLinq, typename R>
    class Max_mixin
    {
    public:
        typename R::value_type max() const
        {
            return boolinq::max(((TLinq<R>*)this)->r);
        }

        template<typename F>
        typename R::value_type max(F f) const
        {
            return boolinq::max(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    // count(xxx) and count(xxx,lambda)

    template<typename R>
    int count(R r)
    {
        int index = 0;
        for (; !r.empty(); r.popFront())
            index++;
        return index;
    }

    template<typename R, typename T>
    int count(R r, T t)
    {
        int index = 0;
        for (; !r.empty(); r.popFront())
            if (r.front() == t)
                index++;
        return index;
    }

    // xxx.count() and xxx.count(lambda)

    template<template<typename> class TLinq, typename R>
    class Count_mixin
    {
    public:
        int count() const
        {
            return boolinq::count(((TLinq<R>*)this)->r);
        }

        template<typename T>
        int count(T t) const
        {
            return boolinq::count(((TLinq<R>*)this)->r,t);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    // toSet(xxx)

    template<typename R>
    std::set<typename R::value_type> toSet(R r)
    {
        std::set<typename R::value_type> result;
        for (; !r.empty(); r.popFront())
            result.insert(r.front());
        return result;
    }

    // xxx.toSet()

    template<template<typename> class TLinq, typename R>
    class ToSet_mixin
    {
    public:
        std::set<typename R::value_type> toSet() const
        {
            return boolinq::toSet(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    // toList(xxx)

    template<typename R>
    std::list<typename R::value_type> toList(R r)
    {
        std::list<typename R::value_type> result;
        for (; !r.empty(); r.popFront())
            result.push_back(r.front());
        return result;
    }

    // xxx.toList()

    template<template<typename> class TLinq, typename R>
    class ToList_mixin
    {
    public:
        std::list<typename R::value_type> toList() const
        {
            return boolinq::toList(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    // for_each(for_each(xxx, ...), ...)

    template<typename R, typename F>
    void for_each(R r, F f)
    {
        while (!r.empty())
            f(r.popFront());
    }

    // xxx.for_each(...).for_each(...)

    template<template<typename> class TLinq, typename R>
    class ForEach_mixin
    {
    public:
        template<typename F>
        void for_each(F f) const
        {
            return boolinq::for_each(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    // toVector(xxx)

    template<typename R>
    std::deque<typename R::value_type> toDeque(R r)
    {
        std::deque<typename R::value_type> result;
        for (; !r.empty(); r.popFront())
            result.push_back(r.front());
        return result;
    }

    // xxx.toVector()

    template<template<typename> class TLinq, typename R>
    class ToDeque_mixin
    {
    public:
        std::deque<typename R::value_type> toDeque() const
        {
            return boolinq::toDeque(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    // toVector(xxx)

    template<typename R>
    std::vector<typename R::value_type> toVector(R r)
    {
        std::vector<typename R::value_type> result;
        for (; !r.empty(); r.popFront())
            result.push_back(r.front());
        return result;
    }

    // xxx.toVector()

    template<template<typename> class TLinq, typename R>
    class ToVector_mixin
    {
    public:
        std::vector<typename R::value_type> toVector() const
        {
            return boolinq::toVector(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    // contains(xxx, value)

    template<typename R, typename T>
    bool contains(R r, const T & t)
    {
        while (!r.empty())
            if (r.popFront() == t)
                return true;
        return false;
    }

    // xxx.contains(value)

    template<template<typename> class TLinq, typename R>
    class Contains_mixin
    {
    public:
        template<typename T>
        bool contains(const T & t) const
        {
            return boolinq::contains(((TLinq<R>*)this)->r,t);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    template<typename T, typename R>
    class CastRange
    {
    public:
        typedef T value_type; 
        
        CastRange(R r)
            : r(r) 
        {
        }

        bool empty()          { return r.empty();    }
        value_type popFront() { return r.popFront(); }
        value_type popBack()  { return r.popBack();  }
        value_type front()    { return r.front();    }
        value_type back()     { return r.back();     }

    private:
        R r;
    };

    // Cast<double>(Cast<int>(xxx))

    template<typename T, typename R>
    CastRange<T,R> cast(R r)
    {
        return r;
    }

    // xxx.Cast<int>().Cast<double>()

    template<template<typename> class TLinq, typename R>
    class CastRange_mixin
    {
    public:
        template<typename T>
        TLinq<CastRange<T,R> > cast() const
        {
            return boolinq::cast<T>(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    template<typename TIter> 
    class IterRange
    {
    public:
        typedef typename std::iterator_traits<TIter>::value_type value_type;
        
        IterRange(TIter b, TIter e)
            : b(b), e(e)
        {
        }

        bool empty()
        {
            return (b == e);
        }

        value_type popFront()
        {
            assert(!empty());
            return *(b++);
        }

        value_type popBack()
        {
            assert(!empty());
            return *(--e);
        }

        value_type front()
        {
            assert(!empty());
            return *b;
        }

        value_type back()
        {
            assert(!empty());
            TIter tmp = e;
            return *(--tmp);
        }

    private:
        TIter b;
        TIter e;
    };

    //////////////////////////////////////////////////////////////////////

    template<typename T>
    IterRange<typename T::const_iterator> range(const T & vec)
    {
        return IterRange<typename T::const_iterator>(vec.begin(), vec.end());
    }
    
    template<typename T, const int N>
    IterRange<const T*> range(const T (&arr)[N])
    {
        return IterRange<const T*>((const T*)arr, (const T*)arr+N);
    }

    template<typename T>
    IterRange<T> range(T b, T e)
    {
        return IterRange<T>(b,e);
    }
    
    template<typename T>
    IterRange<const T*> range(const T * b, const T * e)
    {
        return IterRange<const T*>(b,e);
    }

    template<typename T>
    IterRange<const T*> range(const T * b, int n)
    {
        return IterRange<const T*>(b,b+n);
    }
}
// namespace boolinq

namespace boolinq
{
    // elementAt(xxx, int)

    template<typename R>
    typename R::value_type elementAt(R r, int index)
    {
        while (index > 0)
        {
            r.popFront();
            index--;
        }
        return r.front();
    }

    // xxx.elementAt(int)

    template<template<typename> class TLinq, typename R>
    class ElementAt_mixin
    {
    public:
        typename R::value_type elementAt(int index) const
        {
            return boolinq::elementAt(((TLinq<R>*)this)->r,index);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    template<typename R> 
    class SkipRange
    {
    public:
        typedef typename R::value_type value_type;

        SkipRange(R rng, int n)
            : r(rng)
        {
            for (int i = 0; !r.empty() && (i < n); i++)
                r.popFront();
        }

        bool empty()          { return r.empty();    }
        value_type popFront() { return r.popFront(); }
        value_type popBack()  { return r.popBack();  }
        value_type front()    { return r.front();    }
        value_type back()     { return r.back();     }

    private:
        R r;
    };

    // skip(skip(xxx, ...), ...)

    template<typename R>
    SkipRange<R> skip(R r, int n)
    {
        return SkipRange<R>(r,n);
    }

    // xxx.skip(...).skip(...)

    template<template<typename> class TLinq, typename R>
    class SkipRange_mixin
    {
    public:
        TLinq<SkipRange<R> > skip(int n) const
        {
            return boolinq::skip(((TLinq<R>*)this)->r,n);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    template<typename R> 
    class TakeRange
    {
    public:
        typedef typename R::value_type value_type;

        TakeRange(R r, int n)
            : r(r), n(n), backReady(false)
        {
        }

        bool empty()          
        {
            if ((n == 0) && !backReady)
                return true;
            return r.empty();   
        }

        value_type popFront() 
        {
            n--;
            return r.popFront(); 
        }

        value_type popBack()  
        { 
            if (!backReady)
                prepareBack();
            return r.popBack(); 
        }

        value_type front()    
        { 
            return r.front();   
        }

        value_type back()    
        { 
            if (!backReady)
                prepareBack();
            return r.back();   
        }

    private:
        void prepareBack() 
        {
            int size = boolinq::count(r);
            while (size > n)
            {
                r.popBack();
                size--;
            }
            backReady = true;
        }

    private:
        R r;
        int n;
        bool backReady;
    };

    // take(take(xxx, ...), ...)

    template<typename R>
    TakeRange<R> take(R r, int n)
    {
        return TakeRange<R>(r,n);
    }

    // xxx.take(...).take(...)

    template<template<typename> class TLinq, typename R>
    class TakeRange_mixin
    {
    public:
        TLinq<TakeRange<R> > take(int n) const
        {
            return boolinq::take(((TLinq<R>*)this)->r,n);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    enum ByteOrder
    {
        FirstToLast,
        LastToFirst,
    };

    template<typename R, ByteOrder byteOrder = FirstToLast> 
    class BytesRange
    {
        typedef typename R::value_type old_value_type;
        enum 
        { 
            startByte  = (byteOrder == FirstToLast) ? 0 : (sizeof(old_value_type)-1),
            finishByte = (byteOrder == FirstToLast) ? (sizeof(old_value_type)-1) : 0,
            stepByte   = (byteOrder == FirstToLast) ? 1 : -1,
        };

    public:
        typedef unsigned char value_type;

        BytesRange(R rng)
            : r(rng)
            , frontByte(startByte)
            , backByte(finishByte)
            , atEnd(r.empty())
        {
        }

        bool empty()       
        {
            return atEnd;
        }

        value_type popFront()    
        {
            value_type tmp = front();
            if (checkEmpty())
                return tmp;

            if (frontByte != finishByte)
                frontByte += stepByte;
            else
            {
                frontByte = startByte;
                r.popFront();
            }   

            return tmp; 
        }

        value_type popBack()
        {
            value_type tmp = back();
            if (checkEmpty())
                return tmp;

            if (backByte != startByte)
                backByte -= stepByte;
            else
            {
                backByte = finishByte;
                r.popBack();
            }   

            return tmp;
        }

        value_type front() 
        {
            old_value_type val = r.front();
            return ((unsigned char *)&val)[frontByte];
        }

        value_type back()  
        {
            old_value_type val = r.back();
            return ((unsigned char *)&val)[backByte];     
        }

    private:
        bool checkEmpty()
        {
            R tmp = r;
            tmp.popFront();
            atEnd = tmp.empty() && (frontByte == backByte);
            return atEnd;
        }

    private:
        R r;
        int frontByte;
        int backByte;
        bool atEnd;
    };

    // bytes(xxx)
    // bytes<ByteOrder>(xxx)

    template<typename R>
    BytesRange<R> bytes(R r)
    {
        return r;
    }

    template<ByteOrder byteOrder, typename R>
    BytesRange<R,byteOrder> bytes(R r)
    {
        return r;
    }

    // xxx.bytes(...)
    // xxx.bytes<ByteOrder>(...)

    template<template<typename> class TLinq, typename R>
    class BytesRange_mixin
    {
    public:
        TLinq<BytesRange<R> > bytes() const
        {
            return boolinq::bytes(((TLinq<R>*)this)->r);
        }

        template<ByteOrder byteOrder>
        TLinq<BytesRange<R,byteOrder> > bytes() const
        {
            return boolinq::bytes<byteOrder>(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq



namespace boolinq
{
    enum BitOrder
    {
        HighToLow,
        LowToHigh,
    };

    template<typename R, BitOrder bitOrder = HighToLow> 
    class BitsRange
    {
        typedef typename R::value_type old_value_type;

        enum 
        { 
            startBit  = (bitOrder == LowToHigh) ? 0 : (CHAR_BIT-1),
            finishBit = (bitOrder == LowToHigh) ? (CHAR_BIT-1) : 0,
            stepBit   = (bitOrder == LowToHigh) ? 1 : -1,
        };

    public:
        typedef int value_type;

        BitsRange(R rng)
            : r(rng)
            , frontBit(startBit)
            , backBit(finishBit)
            , atEnd(r.empty())
        {
        }

        bool empty()       
        {
            return atEnd;
        }

        value_type popFront()    
        {
            value_type tmp = front();
            if (checkEmpty())
                return tmp;

            if (frontBit != finishBit)
                frontBit += stepBit;
            else
            {
                frontBit = startBit;
                r.popFront(); 
            }   

            return tmp; 
        }

        value_type popBack()
        {
            value_type tmp = back();
            if (checkEmpty())
                return tmp;
            
            if (backBit != startBit)
                backBit -= stepBit;
            else
            {
                backBit = finishBit;
                r.popBack(); 
            }   

            return tmp;
        }
        
        value_type front() 
        {
            return (r.front() >> frontBit) & 1;
        }

        value_type back()  
        {
            return (r.back() >> backBit) & 1;     
        }

    private:
        bool checkEmpty()
        {
            R tmp = r;
            tmp.popFront();
            atEnd = tmp.empty() && (frontBit == backBit);
            return atEnd;
        }

    private:
        R r;
        int frontBit;
        int backBit;
        bool atEnd;
    };

    // bits(xxx)
    // bits<BitOrder>(xxx)
    // bits<BitOrder,ByteOrder>(xxx)

    template<typename R>
    BitsRange<BytesRange<R> > bits(R r)
    {
        return boolinq::bytes(r);
    }

    template<BitOrder bitOrder, typename R>
    BitsRange<BytesRange<R>,bitOrder> bits(R r)
    {
        return boolinq::bytes(r);
    }

    template<BitOrder bitOrder, ByteOrder byteOrder, typename R>
    BitsRange<BytesRange<R,byteOrder>,bitOrder> bits(R r)
    {
        return boolinq::bytes<byteOrder>(r);
    }

    // xxx.bits()
    // xxx.bits<BitOrder>()
    // xxx.bits<BitOrder,ByteOrder>()

    template<template<typename> class TLinq, typename R>
    class BitsRange_mixin
    {
    public:
        TLinq<BitsRange<BytesRange<R> > > bits() const
        {
            return boolinq::bits(((TLinq<R>*)this)->r);
        }

        template<BitOrder bitOrder>
        TLinq<BitsRange<BytesRange<R>,bitOrder> > bits() const
        {
            return boolinq::bits<bitOrder>(((TLinq<R>*)this)->r);
        }

        template<BitOrder bitOrder, ByteOrder byteOrder>
        TLinq<BitsRange<BytesRange<R,byteOrder>,bitOrder> > bits() const
        {
            return boolinq::bits<bitOrder,byteOrder>(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    template<typename R, typename F> 
    class WhereRange
    {
    public:
        typedef typename R::value_type value_type;
        
        WhereRange(R r, F f)
            : r(r), f(f)
            , frontReady(false)
            , backReady(false)
        {
        }

        bool empty() 
        { 
            if (!frontReady)
                seekFront();
            return r.empty();
        }

        value_type popFront() 
        { 
            if (!frontReady)
                seekFront();

            auto tmp = *this;
            r.popFront();
            frontReady = false;
            return tmp.front();
        }

        value_type popBack() 
        {
            if (!frontReady)
                seekFront();

            auto tmp = *this;
            r.popBack();
            backReady = false;
            return tmp.back();
        }

        value_type front()
        { 
            if (!frontReady)
                seekFront();
            return r.front();
        }

        value_type back() 
        { 
            if (!backReady)
                seekBack();
            return r.back();
        }

    private:
        void seekFront()
        {
            while(!r.empty() && !f(r.front()))
                r.popFront();
            frontReady = true;
        }

        void seekBack()
        {
            while(!r.empty() && !f(r.back()))
                r.popBack();
            backReady = true;
        }

    private:
        R r;
        F f;
        bool frontReady;
        bool backReady;
    };

    // where(where(xxx, ...), ...)

    template<typename R, typename F>
    WhereRange<R,F> where(R r, F f)
    {
        return WhereRange<R,F>(r,f);
    }
    
    // xxx.where(...).where(...)

    template<template<typename> class TLinq, typename R>
    class WhereRange_mixin
    {
    public:
        template<typename F>
        TLinq<WhereRange<R,F> > where(F f) const
        {
            return boolinq::where(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    template<typename R, typename F>
    class SelectRange
    {
        template<typename F, typename TArg>
        static auto get_return_type(F * f = NULL, TArg * arg = NULL)
                    -> decltype((*f)(*arg));
    public:
        typedef decltype(get_return_type<F,typename R::value_type>()) value_type;

        SelectRange(R r, F f)
            : r(r), f(f) 
        {
        }

        bool empty()          { return r.empty(); }
        value_type popFront() { return f(r.popFront()); }
        value_type popBack()  { return f(r.popBack());  }
        value_type front()    { return f(r.front());    }
        value_type back()     { return f(r.back());     }

    private:
        R r;
        F f; 
    };

    // select(select(xxx, ...), ...)

    template<typename R, typename F>
    SelectRange<R,F> select(R r, F f)
    {
        return SelectRange<R,F>(r,f);
    }

    // xxx.select(...).select(...)

    template<template<typename> class TLinq, typename R>
    class SelectRange_mixin
    {
    public:
        template<typename F>
        TLinq<SelectRange<R,F> > select(F f) const
        {
            return boolinq::select(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    // toContainer<std::vector>(xxx)
    // toContainer<QList>(xxx)
    
    template<template<typename> class X, typename R>
    X<typename R::value_type> toContainer(R r)
    {
        X<typename R::value_type> result;
        for (; !r.empty(); r.popFront())
            result.push_back(r.front());
        return result;
    }

    template<template<typename,typename> class X, typename R>
    X<typename R::value_type, std::allocator<typename R::value_type> > toContainer(R r)
    {
        X<typename R::value_type, std::allocator<typename R::value_type> > result;
        for (; !r.empty(); r.popFront())
            result.push_back(r.front());
        return result;
    }

    // xxx.toContainer<std::vector>()
    // xxx.toContainer<QList>()

    template<template<typename> class TLinq, typename R>
    class ToContainer_mixin
    {
        typedef typename R::value_type value_type;

    public:
        template<template<typename> class X>
        X<value_type> toContainer() const
        {
            return boolinq::toContainer<X>(((TLinq<R>*)this)->r);
        }

        template<template<typename,typename> class X>
        X<value_type, std::allocator<value_type> > toContainer() const
        {
            return boolinq::toContainer<X>(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq

//////////////////////////////////////////////////////////////////////////
// Compare Range with array
//////////////////////////////////////////////////////////////////////////

template<typename R, typename T, int N, typename F>
void CheckRangeEqArrayFront(R dst, T (&ans)[N], F f)
{
    for (int i = 0; i < N; i++)
    {
        EXPECT_FALSE(dst.empty());
        EXPECT_EQ(f(ans[i]),   f(dst.front()));
        EXPECT_EQ(f(ans[N-1]), f(dst.back()));
        EXPECT_EQ(f(ans[i]),   f(dst.popFront()));
    }

    EXPECT_TRUE(dst.empty());
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqArrayBack(R dst, T (&ans)[N], F f)
{
    for (int i = N; i > 0; i--)
    {
        EXPECT_FALSE(dst.empty());
        EXPECT_EQ(f(ans[0]),   f(dst.front()));
        EXPECT_EQ(f(ans[i-1]), f(dst.back()));
        EXPECT_EQ(f(ans[i-1]), f(dst.popBack()));
    }

    EXPECT_TRUE(dst.empty());
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqArrayTwisted(R dst, T (&ans)[N], F f, int bit)
{
    int b = 0;
    int e = N-1;

    for (int i = 0; i < N; i++)
    {
        EXPECT_FALSE(dst.empty());
        EXPECT_EQ(f(ans[b]), f(dst.front()));
        EXPECT_EQ(f(ans[e]), f(dst.back()));

        if (bit ^= 1)
            EXPECT_EQ(f(ans[b++]), f(dst.popFront()));
        else
            EXPECT_EQ(f(ans[e--]), f(dst.popBack()));
    }

    EXPECT_TRUE(dst.empty());
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqArrayFrontBack(R dst, T (&ans)[N], F f)
{
    CheckRangeEqArrayTwisted(dst,ans,f,0);
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqArrayBackFront(R dst, T (&ans)[N], F f)
{
    CheckRangeEqArrayTwisted(dst,ans,f,1);
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqArray(R dst, T (&ans)[N], F f)
{
    CheckRangeEqArrayFront(dst,ans,f);
    CheckRangeEqArrayBack(dst,ans,f);
    CheckRangeEqArrayFrontBack(dst,ans,f);
    CheckRangeEqArrayBackFront(dst,ans,f);
}

//////////////////////////////////////////////////////////////////////////

template<typename R, typename T, int N>
void CheckRangeEqArrayFront(R dst, T (&ans)[N])
{
    CheckRangeEqArrayFront(dst, ans, [](const T & a)->T{return a;});
}

template<typename R, typename T, int N>
void CheckRangeEqArrayBack(R dst, T (&ans)[N])
{
    CheckRangeEqArrayBack(dst, ans, [](const T & a)->T{return a;});
}

template<typename R, typename T, int N>
void CheckRangeEqArrayFrontBack(R dst, T (&ans)[N])
{
    CheckRangeEqArrayFrontBack(dst, ans, [](const T & a)->T{return a;});
}

template<typename R, typename T, int N>
void CheckRangeEqArrayBackFront(R dst, T (&ans)[N])
{
    CheckRangeEqArrayBackFront(dst, ans, [](const T & a)->T{return a;});
}

template<typename R, typename T, int N>
void CheckRangeEqArray(R dst, T (&ans)[N])
{
    CheckRangeEqArray(dst, ans, [](const T & a)->T{return a;});
}

//////////////////////////////////////////////////////////////////////////
// Compare Range with set
//////////////////////////////////////////////////////////////////////////

template<typename T, int N>
std::set<T> ArrayToSet(T (&ans)[N])
{
    std::set<T> res;
    for(int i = 0; i < N; i++)
        res.insert(ans[i]);

    EXPECT_EQ(N, res.size());

    return res;
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqSetFront(R r, T (&ans)[N], F f) 
{
    std::set<T> result;
    while (!r.empty())
        result.insert(f(r.popFront()));

    EXPECT_EQ(ArrayToSet(ans),result);
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqSetBack(R r, T (&ans)[N], F f) 
{
    std::set<T> result;
    while (!r.empty())
        result.insert(f(r.popBack()));

    EXPECT_EQ(ArrayToSet(ans),result);
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqSetTwisted(R r, F f, T (&ans)[N], int bit) 
{
    std::set<T> result;
    while (!r.empty())
    {
        if (bit ^= 1)
            result.insert(f(r.popFront()));
        else
            result.insert(f(r.popBack()));
    }

    EXPECT_EQ(ArrayToSet(ans),result);
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqSetFrontBack(R r, T (&ans)[N], F f) 
{
    CheckRangeEqSetTwisted(r,f,ans,0);
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqSetBackFront(R r, T (&ans)[N], F f) 
{
    CheckRangeEqSetTwisted(r,f,ans,1);
}

template<typename R, typename T, int N, typename F>
void CheckRangeEqSet(R r, T (&ans)[N], F f) 
{
    CheckRangeEqSetFront(r,ans,f);
    CheckRangeEqSetBack(r,ans,f);
    CheckRangeEqSetFrontBack(r,ans,f);
    CheckRangeEqSetBackFront(r,ans,f);
}

//////////////////////////////////////////////////////////////////////////

template<typename R, typename T, int N>
void CheckRangeEqSetFront(R r, T (&ans)[N]) 
{
    CheckRangeEqSetFront(r, ans, [](const T & a)->T{return a;});
}

template<typename R, typename T, int N>
void CheckRangeEqSetBack(R r, T (&ans)[N]) 
{
    CheckRangeEqSetBack(r, ans, [](const T & a)->T{return a;});
}

template<typename R, typename T, int N>
void CheckRangeEqSetFrontBack(R r, T (&ans)[N]) 
{
    CheckRangeEqSetFrontBack(r, ans, [](const T & a)->T{return a;});
}

template<typename R, typename T, int N>
void CheckRangeEqSetBackFront(R r, T (&ans)[N]) 
{
    CheckRangeEqSetBackFront(r, ans, [](const T & a)->T{return a;});
}

template<typename R, typename T, int N>
void CheckRangeEqSet(R r, T (&ans)[N])
{
    CheckRangeEqSet(r, ans, [](const T & a)->T{return a;});
}


namespace boolinq
{
    template<typename R1, typename R2> 
    class ConcatRange
    {
        static_assert(std::tr1::is_same<typename R1::value_type,
                                        typename R2::value_type>::value,
                      "Error unioning ranges with different value types");

    public:
        typedef typename R1::value_type value_type;

        ConcatRange(R1 r1, R2 r2)
            : r1(r1), r2(r2)
        {
        }
    
        bool empty() 
        { 
            return r1.empty() && r2.empty();
        }

        value_type popFront()
        {
            if (r1.empty())
                return r2.popFront();
            return r1.popFront();
        }

        value_type popBack()
        {
            if (r2.empty())
                return r1.popBack();
            return r2.popBack();
        }

        value_type front() 
        {
            if (r1.empty())
                return r2.front();
            return r1.front();
        }

        value_type back()
        { 
            if (r2.empty())
                return r1.back();
            return r2.back();
        }

    private:
        R1 r1;
        R2 r2;
    };

    /// unionAll(unionAll(xxx,yyy),zzz)

    template<typename R1, typename R2>
    ConcatRange<R1,R2> unionAll(R1 r1, R2 r2)
    {
        return ConcatRange<R1,R2>(r1,r2);
    }

    /// xxx.unionAll(yyy).unionAll(zzz)

    template<template<typename> class TLinq, typename R1>
    class ConcatRange_mixin
    {
    public:
        template<typename R2>
        TLinq<ConcatRange<R1,R2> > unionAll(R2 r) const
        {
            return boolinq::unionAll(((TLinq<R1>*)this)->r,r);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    template<typename R>
    class ReverseRange
    {
    public:
        typedef typename R::value_type value_type; 
        
        ReverseRange(R r)
            : r(r) 
        {
        }

        bool empty()          { return r.empty();    }
        value_type popFront() { return r.popBack();  }
        value_type popBack()  { return r.popFront(); }
        value_type front()    { return r.back();     }
        value_type back()     { return r.front();    }

        template<typename R2>
        friend R2 reverse(ReverseRange<R2> r); // smart needed

    private:
        R r;
    };

    // reverse(reverse(xxx))

    template<typename R>
    ReverseRange<R> reverse(R r)
    {
        return r;
    }

    // Unwrap for double-reverse case
    template<typename R>
    R reverse(ReverseRange<R> r)
    {
        return r.r; // smart
    }

    // xxx.reverse().reverse()

    template<template<typename> class TLinq, typename R>
    class ReverseRange_mixin
    {
    public:
        TLinq<ReverseRange<R> > reverse() const
        {
            return boolinq::reverse(((TLinq<R>*)this)->r);
        }
    };

    // Unwrap for double-reverse case
    template<template<typename> class TLinq, typename T>
    class ReverseRange_mixin<TLinq,ReverseRange<T> >
    {
    public:
        TLinq<T> reverse() const
        {
            return boolinq::reverse(((TLinq<ReverseRange<T> >*)this)->r);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    template<typename R, typename T, ByteOrder byteOrder = FirstToLast> 
    class UnbytesRange
    {
        enum 
        { 
            startByte  = (byteOrder == FirstToLast) ? 0 : (sizeof(T)-1),
            finishByte = (byteOrder == FirstToLast) ? (sizeof(T)-1) : 0,
            stepByte   = (byteOrder == FirstToLast) ? 1 : -1,
        };

    public:
        typedef T value_type;

        UnbytesRange(R rng)
            : r(rng)
            , frontValue()
            , backValue()
            , preEnd(r.empty())
            , atEnd(r.empty())
        {
            if (!atEnd)
            {
                popFront();
                popBack();
            }
        }

        bool empty()       
        {
            return atEnd;
        }

        value_type popFront()    
        {
            value_type tmp = front();
            
            if (preEnd)
            {
                atEnd = true;
                return tmp;
            }

            if (r.empty())
            {
                preEnd = true;
                frontValue = backValue;
            }
            else
            {
                for (int i = startByte; i != finishByte+stepByte; i += stepByte)
                    ((unsigned char*)&frontValue)[i] = r.popFront();
            }

            return tmp; 
        }

        value_type popBack()
        {
            value_type tmp = back();
            
            if (preEnd)
            {
                atEnd = true;
                return tmp;
            }

            if (r.empty())
            {
                preEnd = true;
                backValue = frontValue;
            }
            else
            {
                for (int i = finishByte; i != startByte-stepByte; i -= stepByte)
                    ((unsigned char*)&backValue)[i] = r.popBack();
            }

            return tmp;
        }

        value_type front() 
        {
            return frontValue;
        }

        value_type back()  
        {
            return backValue;
        }

    private:
        R r;
        value_type frontValue;
        value_type backValue;
        bool preEnd;
        bool atEnd;
    };

    // unbytes(xxx)
    // unbytes<ByteOrder>(xxx)

    template<typename T, typename R>
    UnbytesRange<R,T> unbytes(R r)
    {
        return r;
    }

    template<typename T, ByteOrder byteOrder, typename R>
    UnbytesRange<R,T,byteOrder> unbytes(R r)
    {
        return r;
    }

    // xxx.unbytes(...)
    // xxx.unbytes<ByteOrder>(...)

    template<template<typename> class TLinq, typename R>
    class UnbytesRange_mixin
    {
    public:
        template<typename T>
        TLinq<UnbytesRange<R,T> > unbytes() const
        {
            return boolinq::unbytes<T>(((TLinq<R>*)this)->r);
        }

        template<typename T, ByteOrder byteOrder>
        TLinq<UnbytesRange<R,T,byteOrder> > unbytes() const
        {
            return boolinq::unbytes<T,byteOrder>(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    template<typename R, BitOrder bitOrder = HighToLow> 
    class UnbitsRange
    {
        enum 
        { 
            startBit  = (bitOrder == LowToHigh) ? 0 : (CHAR_BIT-1),
            finishBit = (bitOrder == LowToHigh) ? (CHAR_BIT-1) : 0,
            stepBit   = (bitOrder == LowToHigh) ? 1 : -1,
        };

    public:
        typedef unsigned char value_type;

        UnbitsRange(R rng)
            : r(rng)
            , frontValue()
            , backValue()
            , preEnd(r.empty())
            , atEnd(r.empty())
        {
            if (!atEnd)
            {
                popFront();
                popBack();
            }
        }

        bool empty()       
        {
            return atEnd;
        }

        value_type popFront()    
        {
            value_type tmp = front();

            if (preEnd)
            {
                atEnd = true;
                return tmp;
            }

            if (r.empty())
            {
                preEnd = true;
                frontValue = backValue;
            }
            else
            {
                frontValue = 0;
                for (int i = startBit; !r.empty() && i != finishBit+stepBit; i += stepBit)
                    frontValue |= ((r.popFront()&1) << i);
            }

            return tmp; 
        }

        value_type popBack()
        {
            value_type tmp = back();

            if (preEnd)
            {
                atEnd = true;
                return tmp;
            }

            if (r.empty())
            {
                preEnd = true;
                backValue = frontValue;
            }
            else
            {
                backValue = 0;
                for (int i = finishBit; !r.empty() && i != startBit-stepBit; i -= stepBit)
                    backValue |= ((r.popBack()&1) << i);
            }

            return tmp;
        }

        value_type front() 
        {
            return frontValue;
        }

        value_type back()  
        {
            return backValue;
        }

    private:
        R r;
        value_type frontValue;
        value_type backValue;
        bool preEnd;
        bool atEnd;
    };

    // unbits(xxx)
    // unbits<BitOrder>(xxx)
    // unbits<T>(xxx)
    // unbits<T,BitOrder>(xxx)
    // unbits<T,ByteOrder>(xxx)
    // unbits<T,BitOrder,ByteOrder>(xxx)

    template<typename R>
    UnbitsRange<R> unbits(R r)
    {
        return r;
    }

    template<BitOrder bitOrder, typename R>
    UnbitsRange<R,bitOrder> unbits(R r)
    {
        return r;
    }

    template<typename T, typename R>
    UnbytesRange<UnbitsRange<R>,T> unbits(R r)
    {
        return r;
    }

    template<typename T, BitOrder bitOrder, typename R>
    UnbytesRange<UnbitsRange<R,bitOrder>,T> unbits(R r)
    {
        return r;
    }

    //template<typename T, ByteOrder byteOrder, typename R>
    //UnbytesRange<UnbitsRange<R>,T,byteOrder> unbits(R r)
    //{
    //    return r;
    //}
    
    template<typename T, BitOrder bitOrder, ByteOrder byteOrder, typename R>
    UnbytesRange<UnbitsRange<R,bitOrder>,T,byteOrder> unbits(R r)
    {
        return r;
    }

    // xxx.unbits()
    // xxx.unbits<BitOrder>()
    // xxx.unbits<T>()
    // xxx.unbits<T,BitOrder>()
    // xxx.unbits<T,ByteOrder>()
    // xxx.unbits<T,BitOrder,ByteOrder>()

    template<template<typename> class TLinq, typename R>
    class UnbitsRange_mixin
    {
    public:
        TLinq<UnbitsRange<R> > unbits() const
        {
            return boolinq::unbits(((TLinq<R>*)this)->r);
        }

        template<BitOrder bitOrder>
        TLinq<UnbitsRange<R,bitOrder> > unbits() const
        {
            return boolinq::unbits<bitOrder>(((TLinq<R>*)this)->r);
        }

        template<typename T>
        TLinq<UnbytesRange<UnbitsRange<R>,T> > unbits() const
        {
            return boolinq::unbits<T>(((TLinq<R>*)this)->r);
        }

        template<typename T, BitOrder bitOrder>
        TLinq<UnbytesRange<UnbitsRange<R,bitOrder>,T> > unbits() const
        {
            return boolinq::unbits<T,bitOrder>(((TLinq<R>*)this)->r);
        }

        //template<typename T, ByteOrder byteOrder>
        //TLinq<UnbytesRange<UnbitsRange<R>,T,byteOrder> > unbits() const
        //{
        //    return boolinq::unbits<T,byteOrder>(((TLinq<R>*)this)->r);
        //}

        template<typename T, BitOrder bitOrder, ByteOrder byteOrder>
        TLinq<UnbytesRange<UnbitsRange<R,bitOrder>,T,byteOrder> > unbits() const
        {
            return boolinq::unbits<T,bitOrder,byteOrder>(((TLinq<R>*)this)->r);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    template<typename T>
    struct JustReturn
    {
        T operator()(const T & a) const
        {
            return a;
        }
    };

    template<typename R, typename F = JustReturn<typename R::value_type> > 
    class OrderByRange
    {
    public:
        typedef typename R::value_type value_type;

        OrderByRange(R r, F f = JustReturn<typename R::value_type>())
            : r(r), f(f)
            , atEnd(false)
            , size(boolinq::count(r))
            , minimumValue(r)
            , maximumValue(r)
            , minimumIndex(0)
            , maximumIndex(0)
        {
            seekFirstMin();
            seekFirstMax();
            atEnd = (minimumIndex + maximumIndex == size);
        }

        bool empty() 
        { 
            return atEnd;
        }

        value_type popFront() 
        { 
            R tmp = minimumValue;
            seekNextMin();
            return tmp.front();
        }

        value_type popBack() 
        {
            R tmp = maximumValue;
            seekNextMax();
            return tmp.back();
        }

        value_type front() 
        { 
            return minimumValue.front();
        }

        value_type back() 
        { 
            return maximumValue.back();
        }

    private:
        void seekFirstMin()
        {
            R currentValue = r;
            int currentIndex = 0;
            
            while(!currentValue.empty())
            {
                if (f(currentValue.front()) < f(minimumValue.front()))
                {
                    minimumValue = currentValue;
                    minimumIndex = currentIndex;
                }

                currentValue.popFront();
                currentIndex++;
            }
        }

        void seekFirstMax()
        {
            R currentValue = r;
            int currentIndex = 0;

            while(!currentValue.empty())
            {
                if (f(maximumValue.back()) < f(currentValue.back()))
                {
                    maximumValue = currentValue;
                    maximumIndex = currentIndex;
                }

                currentValue.popBack();
                currentIndex++;
            }
        }

        void seekNextMin()
        {
            if (minimumIndex + maximumIndex + 1 == size)
            {
                atEnd = true;
                return;
            }

            R cur_value = r;
            R min_value = minimumValue;
            int cur_index = 0;
            int min_index = minimumIndex;

            while(!cur_value.empty())
            {
                if ((f(cur_value.front()) < f(minimumValue.front()))
                    || (f(cur_value.front()) == f(minimumValue.front())
                        && cur_index <= minimumIndex))
                {
                    cur_value.popFront();
                    cur_index++;
                    continue;
                }

                if (min_index == minimumIndex
                    && cur_index != minimumIndex)
                {
                    min_value = cur_value;
                    min_index = cur_index;
                }
                
                if (f(cur_value.front()) < f(min_value.front()))       
                {
                    min_value = cur_value;
                    min_index = cur_index;
                }

                if (f(cur_value.front()) == f(minimumValue.front())
                    && minimumIndex < cur_index)
                {
                    minimumValue = cur_value;
                    minimumIndex = cur_index;
                    return;

                }

                cur_value.popFront();
                cur_index++;
            }

            minimumValue = min_value;
            minimumIndex = min_index;
        }

        void seekNextMax()
        {
            if (minimumIndex + maximumIndex + 1 == size)
            {
                atEnd = true;
                return;
            }

            R cur_value = r;
            R max_value = maximumValue;
            int cur_index = 0;
            int max_index = maximumIndex;

            while(!cur_value.empty())
            {
                if ((f(maximumValue.back()) < f(cur_value.back()))
                    || (f(cur_value.back()) == f(maximumValue.back())
                        && cur_index <= maximumIndex))
                {
                    cur_value.popBack();
                    cur_index++;
                    continue;
                }

                if (max_index == maximumIndex
                    && cur_index != maximumIndex)
                {
                    max_value = cur_value;
                    max_index = cur_index;
                }

                if (f(max_value.back()) < f(cur_value.back()))
                {
                    max_value = cur_value;
                    max_index = cur_index;
                }

                if (f(cur_value.back()) == f(maximumValue.back())
                    && maximumIndex < cur_index)
                {
                    maximumValue = max_value;
                    maximumIndex = max_index;
                    return;
                }

                cur_value.popBack();
                cur_index++;
            }

            maximumValue = max_value;
            maximumIndex = max_index;  
        }

    private:
        R r;
        F f;

        bool atEnd;
        int size;
        R minimumValue;
        R maximumValue;
        int minimumIndex;
        int maximumIndex;
    };

    // orderBy(orderBy(xxx, ...), ...)

    template<typename R>
    OrderByRange<R> orderBy(R r)
    {
        return r;
    }

    template<typename R, typename F>
    OrderByRange<R,F> orderBy(R r, F f)
    {
        return OrderByRange<R,F>(r,f);
    }

    // xxx.orderBy(...).orderBy(...)

    template<template<typename> class TLinq, typename R>
    class OrderByRange_mixin
    {
    public:
        TLinq<OrderByRange<R> > orderBy() const
        {
            return boolinq::orderBy(((TLinq<R>*)this)->r);
        }

        template<typename F>
        TLinq<OrderByRange<R,F> > orderBy(F f) const
        {
            return boolinq::orderBy(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq

namespace boolinq
{
    template<typename T>
    struct JustReturn_dist
    {
        T operator()(const T & a) const
        {
            return a;
        }
    };

    template<typename R, typename F = JustReturn_dist<typename R::value_type> > 
    class DistinctRange
    {
    public:
        typedef typename R::value_type value_type;

        DistinctRange(R r, F f = JustReturn_dist<typename R::value_type>())
            : r(r), f(f)
            , fullRange(r)
            , leftIndex(0)
            , rightIndex(0)
        {
            if (isDuplicate(f(r.front()),1,0))
                popFront();
        }

        bool empty() 
        { 
            return r.empty();
        }

        value_type popFront() 
        { 
            R tmp = r;
            do 
            {
                r.popFront();
                leftIndex++;
            } while (!r.empty() && isDuplicate(f(r.front()),1,0));
            return tmp.front();
        }

        value_type popBack() 
        {
            R tmp = r;
            do
            {
                r.popBack();
                rightIndex++;
            } while (!r.empty() && isDuplicate(f(r.back()),0,1));
            return tmp.back();
        }

        value_type front() 
        { 
            return r.front();
        }

        value_type back() 
        { 
            return r.back();
        }

    private:
        template<typename T>
        bool isDuplicate(const T & value, int left, int right) const
        {
            R tmp = r;
            tmp.popFront();
            if (tmp.empty())
                return false;

            tmp = fullRange;
            for(int i = 0 ; i < leftIndex + right; i++)
            {
                if (value == f(tmp.popFront()))
                    return true;
            }

            tmp = fullRange;
            for(int i = 0 ; i < rightIndex + left; i++)
            {
                if (value == f(tmp.popBack()))
                    return true;
            }

            return false;
        }

    private:
        R r;
        F f;

        R fullRange;
        int leftIndex;
        int rightIndex;
    };

    // distinct(distinct(xxx))

    template<typename R>
    DistinctRange<R> distinct(R r)
    {
        return r;
    }

    template<typename R, typename F>
    DistinctRange<R,F> distinct(R r, F f)
    {
        return DistinctRange<R,F>(r,f);
    }

    // xxx.distinct().distinct()

    template<template<typename> class TLinq, typename R>
    class DistinctRange_mixin
    {
    public:
        TLinq<DistinctRange<R> > distinct() const
        {
            return boolinq::distinct(((TLinq<R>*)this)->r);
        }

        template<typename F>
        TLinq<DistinctRange<R,F> > distinct(F f) const
        {
            return boolinq::distinct(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq


namespace boolinq
{
    template<typename F, typename TArg>
    static auto get_return_type_gb(F * f = NULL, TArg * arg = NULL)
                -> decltype((*f)(*arg));

    template<typename T>
    struct JustReturn_gb
    {
        T operator()(const T & a) const
        {
            return a;
        }
    };

    template<typename T, typename F>
    struct ReturnEquals_gb
    {
        typedef decltype(get_return_type_gb<F,T>()) value_type;

        ReturnEquals_gb(const T & val, F f)
            : val(f(val)), f(f)
        {
        }

        bool operator()(const T & a) const
        {
            return val == f(a);
        }

    private:
        value_type val;
        F f;
    };

    template<typename R, typename F> 
    class WhereRange_withKey : public WhereRange<R, ReturnEquals_gb<typename R::value_type,F> >
    {
    public:
        typedef typename R::value_type value_type;
        typedef decltype(get_return_type_gb<F,value_type>()) TKey;

        WhereRange_withKey(R r, F f, const value_type & key)
            : WhereRange(r, ReturnEquals_gb<value_type,F>(key,f))
            , k(f(key))
        {
        }

        const TKey & key() const
        {
            return k;
        }

    private:
        TKey k;
    };

    //////////////////////////////////////////////////////////////////////////

    template<typename R, typename F = JustReturn_gb<typename R::value_type> > 
    class GroupByRange
    {
    public:
        typedef typename R::value_type r_value_type;
        typedef WhereRange_withKey<R,F> value_type;

        GroupByRange(R r, F f = JustReturn_gb<r_value_type>())
            : r(r), dr(r,f), f(f)
            , _front(r,f,dr.front())
            , _back(r,f,dr.back())
        {
        }

        bool empty() 
        { 
            return dr.empty();
        }

        value_type popFront()             
        { 
            DistinctRange<R,F> tmp = dr;
            dr.popFront();
            if ((!empty()))
                _front = value_type(r,f,dr.front());
            return value_type(r,f,tmp.front());
        }

        value_type popBack()
        {
            DistinctRange<R,F> tmp = dr;
            dr.popBack();
            if ((!empty()))
                _back = value_type(r,f,dr.back());
            return value_type(r,f,tmp.back());
        }

        value_type & front()
        { 
            return _front;
        }

        value_type & back()
        { 
            return _back;
        }

    private:
        R r;
        DistinctRange<R,F> dr;
        F f;

        value_type _front;
        value_type _back;
    };

    // groupBy(groupBy(xxx, ...), ...)

    template<typename R, typename F>
    GroupByRange<R,F> groupBy(R r, F f)
    {
        return GroupByRange<R,F>(r,f);
    }

    // xxx.groupBy(...).groupBy(...)

    template<template<typename> class TLinq, typename R>
    class GroupByRange_mixin
    {
    public:
        template<typename F>
        TLinq<GroupByRange<R,F> > groupBy(F f) const
        {
            return boolinq::groupBy(((TLinq<R>*)this)->r,f);
        }
    };
}
// namespace boolinq






namespace boolinq
{
    template<typename R>
    class Linq
        : public CastRange_mixin<Linq,R>
        , public SkipRange_mixin<Linq,R>
        , public TakeRange_mixin<Linq,R>
        , public WhereRange_mixin<Linq,R>
        , public SelectRange_mixin<Linq,R>
        , public ReverseRange_mixin<Linq,R>
        , public OrderByRange_mixin<Linq,R>
        , public GroupByRange_mixin<Linq,R>
        , public DistinctRange_mixin<Linq,R>
        , public ConcatRange_mixin<Linq,R>
        
        , public All_mixin<Linq,R>
        , public Any_mixin<Linq,R>
        , public Sum_mixin<Linq,R>
        , public Avg_mixin<Linq,R>
        , public Min_mixin<Linq,R>
        , public Max_mixin<Linq,R>
        , public Count_mixin<Linq,R>
        , public ForEach_mixin<Linq,R>
        , public Contains_mixin<Linq,R>
        , public ElementAt_mixin<Linq,R>

        , public ToSet_mixin<Linq,R>
        , public ToList_mixin<Linq,R>
        , public ToDeque_mixin<Linq,R>
        , public ToVector_mixin<Linq,R>
        , public ToContainer_mixin<Linq,R>

        , public BytesRange_mixin<Linq,R>
        , public BitsRange_mixin<Linq,R>
        , public UnbytesRange_mixin<Linq,R>
        , public UnbitsRange_mixin<Linq,R>
    {
    public:
        typedef typename R::value_type value_type;

        Linq(const R & r)
            : r(r)
        {
        }

        operator R () const
        {
            return r;
        }

        operator R & ()
        {
            return r;
        }

        bool empty()          { return r.empty();    }
        value_type popFront() { return r.popFront(); }
        value_type popBack()  { return r.popBack();  }
        value_type front()    { return r.front();    }
        value_type back()     { return r.back();     }

    public:
        R r;
    };

    //////////////////////////////////////////////////////////////////////////
    // from<CustomLinq>(xxx)
    //////////////////////////////////////////////////////////////////////////

    template<template<typename> class TLinq, typename X>
    TLinq<IterRange<typename X::const_iterator> > from(const X & x)
    {
        return range(x);
    }
    
    template<template<typename> class TLinq, typename X, const int N>
    TLinq<IterRange<const X*> > from(const X (&x)[N])
    {
        return range(x);
    }
    
    template<template<typename> class TLinq, typename X>
    TLinq<IterRange<X> > from(X b, X e)
    {
        return range(b,e);
    }
    
    template<template<typename> class TLinq, typename X>
    TLinq<IterRange<const X*> > from(const X * b, const X * e)
    {
        return range(b,e);
    }

    template<template<typename> class TLinq, typename X>
    TLinq<IterRange<const X*> > from(const X * b, int n)
    {
        return range(b,n);
    }

    //////////////////////////////////////////////////////////////////////////
    // from(xxx)
    //////////////////////////////////////////////////////////////////////////

    template<typename X>
    Linq<IterRange<typename X::const_iterator> > from(const X & x)
    {
        return from<Linq>(x);
    }

    template<typename X, const int N>
    Linq<IterRange<const X*> > from(const X (&x)[N])
    {
        return from<Linq>(x);
    }

    template<typename X>
    Linq<IterRange<X> > from(X b, X e)
    {
        return from<Linq>(b,e);
    }

    template<typename X>
    Linq<IterRange<const X*> > from(const X * b, const X * e)
    {
        return from<Linq>(b,e);
    }

    template<typename X>
    Linq<IterRange<const X*> > from(const X * b, int n)
    {
        return from<Linq>(b,n);
    }

    //////////////////////////////////////////////////////////////////////////
    // Linq equality operator for container and array
    //////////////////////////////////////////////////////////////////////////

    template<typename R, typename X>
    bool operator == (const Linq<R> & rng, const X & x)
    {
        Linq<R> tmp = rng;
        for (auto it = x.begin(); it != x.end(); ++it)
        {
            if (tmp.empty())
                return false;
            if (tmp.popFront() != *it)
                return false;
        }

        return tmp.empty();
    }

    template<typename R, typename X>
    bool operator == (const X & x, const Linq<R> & rng)
    {
        return rng == x;
    }

    template<typename R, typename X, const int N>
    bool operator == (const Linq<R> & rng, const X (&x)[N])
    {
        Linq<R> tmp = rng;
        for (int i = 0; i < N; i++)
        {
            if (tmp.empty())
                return false;
            if (tmp.popFront() != x[i])
                return false;
        }

        return tmp.empty();
    }

    template<typename R, typename X, const int N>
    bool operator == (const X (&x)[N], const Linq<R> & rng)
    {
        return rng == x;
    }
}
// namespace boolinq
