﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using QuickFIX.NET;
using QuickFIX.NET.Fields;

namespace UnitTests
{
    public class MockFieldMap : FieldMap 
    {
        public MockFieldMap() { }
        public MockFieldMap(int[] fo) : base(fo) { }
    }

    [TestFixture]
    public class FieldMapTests
    {
        private MockFieldMap fieldmap;
        public FieldMapTests()
        {
            this.fieldmap = new MockFieldMap();
        }

        [Test]
        public void CharFieldTest()
        {
           
            CharField field = new CharField(100,'d');
            fieldmap.setField(field);
            CharField refield = new CharField(100);
            fieldmap.getField(refield);
            Assert.That('d', Is.EqualTo(refield.Obj));
            field.Obj = 'e';
            fieldmap.setField(field);
            fieldmap.getField(refield);
            Assert.That('e', Is.EqualTo(refield.Obj));
        }

        [Test]
        public void StringFieldTest()
        {

            fieldmap.setField(new Account("hello"));
            Account acct = new Account();
            fieldmap.getField(acct);
            Assert.That("hello", Is.EqualTo(acct.Obj));
            fieldmap.setField(new Account("helloworld"));
            fieldmap.getField(acct);
            Assert.That("helloworld", Is.EqualTo(acct.getValue()));
        }

        [Test]
        public void DateTimeFieldTest()
        {

            fieldmap.setField(new DateTimeField(Tags.TransactTime,new DateTime(2009,12,10)));
            TransactTime tt= new TransactTime();
            fieldmap.getField(tt);
            Assert.That(new DateTime(2009,12,10), Is.EqualTo(tt.Obj));
            fieldmap.setField(new TransactTime(new DateTime(2010,12,10)));
            fieldmap.getField(tt);
            Assert.That(new DateTime(2010,12,10), Is.EqualTo(tt.getValue()));
        }

        [Test]
        public void BooleanFieldTest()
        {
            BooleanField field = new BooleanField(200, true);
            BooleanField refield = new BooleanField(200);
            fieldmap.setField(field);
            fieldmap.getField(refield);
            Assert.That(true, Is.EqualTo(refield.Obj));
            field.setValue(false);
            fieldmap.setField(field);
            fieldmap.getField(refield);
            Assert.That(false, Is.EqualTo(refield.Obj));
        }

        [Test]
        public void IntFieldTest()
        {

            IntField field = new IntField(200, 101);
            IntField refield = new IntField(200);
            fieldmap.setField(field);
            fieldmap.getField(refield);
            Assert.That(101, Is.EqualTo(refield.Obj));
            field.setValue(102);
            fieldmap.setField(field);
            fieldmap.getField(refield);
            Assert.That(102, Is.EqualTo(refield.Obj));
        }

        [Test]
        public void DecimalFieldTest()
        {
            DecimalField field = new DecimalField(200, new Decimal(101.0001));
            DecimalField refield = new DecimalField(200);
            fieldmap.setField(field);
            fieldmap.getField(refield);
            Assert.That(101.0001, Is.EqualTo(refield.Obj));
            field.setValue(new Decimal(101.0002));
            fieldmap.setField(field);
            fieldmap.getField(refield);
            Assert.That(101.0002, Is.EqualTo(refield.Obj));
        }

        [Test]
        public void DefaultFieldTest()
        {
            DecimalField field = new DecimalField(200, new Decimal(101.0001));
            fieldmap.setField(field);
            string refield = fieldmap.GetField(200);
            Assert.That("101.0001", Is.EqualTo(refield));
     
        }

        [Test]
        public void SetFieldOverwriteTest()
        {
            IntField field = new IntField(21901, 1011);
            IntField refield = new IntField(21901);
            fieldmap.setField(field, false);
            fieldmap.getField(refield);
            Assert.That(1011, Is.EqualTo(refield.Obj));
            field.setValue(1021);
            IntField refield2 = new IntField(21901);
            fieldmap.setField(field, false);
            fieldmap.getField(refield2);
            Assert.That(refield.Obj, Is.EqualTo(1011));
            fieldmap.setField(field, true);
            IntField refield3 = new IntField(21901);
            fieldmap.getField(refield3);
            Assert.That(1021, Is.EqualTo(refield3.Obj));
        }

        [Test]
        public void FieldNotFoundTest()
        {
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.GetField(99900); });
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.getField(new DateTimeField(1002030)); });
            Assert.Throws(typeof(FieldNotFoundException),
                 delegate { fieldmap.getField(new CharField(23099)); });
            Assert.Throws(typeof(FieldNotFoundException),
                 delegate { fieldmap.getField(new BooleanField(99900)); });
            Assert.Throws(typeof(FieldNotFoundException),
                 delegate { fieldmap.getField(new StringField(99900)); });
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.getField(new IntField(99900)); });
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.getField(new DecimalField(99900)); });
        }

        [Test]
        public void SimpleFieldOrderTest()
        {
            int[] fieldord = {10, 11 ,12 ,13, 200};
            MockFieldMap fm = new MockFieldMap(fieldord);
            Assert.That(fm.FieldOrder, Is.EqualTo(fieldord));
        }

        [Test]
        public void AddGetGroupTest()
        {
            Group g1 = new Group(100, 200);
            Group g2 = new Group(100, 201);
            MockFieldMap fm = new MockFieldMap();
            fm.AddGroup(g1);
            fm.AddGroup(g2);
            Assert.That(fm.GetGroup(1, 100), Is.EqualTo(g1));
            Assert.That(fm.GetGroup(2, 100), Is.EqualTo(g2));

            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.GetGroup(0, 101); });
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.GetGroup(3,100); });
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.GetGroup(1, 101); });
        }

        [Test]
        public void RemoveGroupTest()
        {
            Group g1 = new Group(100, 200);
            Group g2 = new Group(100, 201);
            MockFieldMap fm = new MockFieldMap();
            fm.AddGroup(g1);
            fm.AddGroup(g2);
            Assert.That(fm.GetGroup(1, 100), Is.EqualTo(g1));
            Assert.That(fm.GetGroup(2, 100), Is.EqualTo(g2));

            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.RemoveGroup(0, 101); });
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.RemoveGroup(3, 100); });
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.RemoveGroup(1, 101); });

            fm.RemoveGroup(1, 100);
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.GetGroup(2, 100); });
            fm.RemoveGroup(1, 100);
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.GetGroup(1, 100); });
        }

        [Test]
        public void ReplaceGroupTest()
        {
            Group g1 = new Group(100, 200);
            Group g2 = new Group(100, 201);
            MockFieldMap fm = new MockFieldMap();
            fm.AddGroup(g1);
            fm.AddGroup(g2);
            Assert.That(fm.GetGroup(1, 100), Is.EqualTo(g1));
            Assert.That(fm.GetGroup(2, 100), Is.EqualTo(g2));

            Group g3 = new Group(100, 202);
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.ReplaceGroup(0, 101, g3); });
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.ReplaceGroup(3, 100, g3); });
            Assert.Throws(typeof(FieldNotFoundException),
                delegate { fieldmap.ReplaceGroup(1, 101, g3); });

            fm.ReplaceGroup(1, 100, g3);
            fm.ReplaceGroup(2, 100, g3);
            Assert.That(fm.GetGroup(1, 100), Is.EqualTo(g3));
            Assert.That(fm.GetGroup(2, 100), Is.EqualTo(g3));
        }
    }
}