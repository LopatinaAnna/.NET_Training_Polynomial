using System.Collections.Generic;
using PolynomialObject.Exceptions;

namespace PolynomialObject
{
    public sealed class Polynomial
    {
        public List<PolynomialMember> Members { get; set; } = new List<PolynomialMember>();

        public Polynomial() { }

        public Polynomial(PolynomialMember member)
        {
            if (member.Coefficient != 0)
                AddMember(member);
        }

        public Polynomial(IEnumerable<PolynomialMember> members)
        {
            foreach (var member in members)
            {
                if (member.Coefficient != 0)
                    AddMember(member);
            }
        }

        public Polynomial((double degree, double coefficient) member)
        {
            if (member.coefficient != 0)
                AddMember((member.degree, member.coefficient));
        }

        public Polynomial(IEnumerable<(double degree, double coefficient)> members)
        {
            foreach (var (degree, coefficient) in members)
            {
                if ((degree, coefficient).coefficient != 0)
                    AddMember((degree, coefficient));
            }
        }

        /// <summary>
        /// The amount of not null polynomial members in polynomial 
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                foreach (var item in Members)
                {
                    if (item != null)
                        ++count;
                }
                return count;
            }
        }

        /// <summary>
        /// The biggest degree of polynomial member in polynomial
        /// </summary>
        public double Degree
        {
            get
            {
                double max = default;
                foreach (var item in Members)
                {
                    if (item.Degree > max)
                        max = item.Degree;
                }
                return max;
            }
        }

        /// <summary>
        /// Adds new unique member to polynomial 
        /// </summary>
        /// <param name="member">The member to be added</param>
        /// <exception cref="PolynomialArgumentException">Throws when member to add with such degree already exist in polynomial</exception>
        /// <exception cref="PolynomialArgumentNullException">Throws when trying to member to add is null</exception>
        public void AddMember(PolynomialMember member)
        {
            if (member == null)
                throw new PolynomialArgumentNullException("Member to add is null");
            if (member.Coefficient == 0)
                throw new PolynomialArgumentException("Coefficient is zero");
            if (ContainsMember(member.Degree))
                throw new PolynomialArgumentException("Member to add with such degree already exist in polynomial");

            Members.Add(member);
        }

        /// <summary>
        /// Adds new unique member to polynomial from tuple
        /// </summary>
        /// <param name="member">The member to be added</param>
        /// <exception cref="PolynomialArgumentException">Throws when member to add with such degree already exist in polynomial</exception>
        public void AddMember((double degree, double coefficient) member)
        {
            if (member.coefficient == 0)
                throw new PolynomialArgumentException("Coefficient is zero");
            if (ContainsMember(member.degree))
                throw new PolynomialArgumentException("Member to add with such degree already exist in polynomial");

            Members.Add(new PolynomialMember(member.degree, member.coefficient));
        }

        /// <summary>
        /// Removes member of specified degree
        /// </summary>
        /// <param name="degree">The degree of member to be deleted</param>
        /// <returns>True if member has been deleted</returns>
        public bool RemoveMember(double degree)
        {
            foreach (var item in Members)
            {
                if (item.Degree == degree)
                {
                    Members.Remove(item);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Searches the polynomial for a method of specified degree
        /// </summary>
        /// <param name="degree">Degree of member</param>
        /// <returns>True if polynomial contains member</returns>
        public bool ContainsMember(double degree)
        {
            foreach (var item in Members)
            {
                if (item.Degree == degree)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds member of specified degree
        /// </summary>
        /// <param name="degree">Degree of member</param>
        /// <returns>Returns the found member or null</returns>
        public PolynomialMember Find(double degree)
        {
            foreach (var item in Members)
            {
                if (item.Degree == degree)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets and sets the coefficient of member with provided degree
        /// If there is no null member for searched degree - return 0 for get and add new member for set
        /// </summary>
        /// <param name="degree">The degree of searched member</param>
        /// <returns>Coefficient of found member</returns>
        public double this[double degree]
        {
            get
            {
                if (ContainsMember(degree))
                    return Find(degree).Coefficient;
                else return 0;
            }
            set
            {
                if (ContainsMember(degree) && value != 0)
                    Find(degree).Coefficient = value;
                else
                {
                    if (value != 0)
                        AddMember((degree, value));
                    else 
                        RemoveMember(degree);
                }
            }
        }

        /// <summary>
        /// Convert polynomial to array of included polynomial members 
        /// </summary>
        /// <returns>Array with not null polynomial members</returns>
        public PolynomialMember[] ToArray()
        {
            return Members.ToArray();
        }

        /// <summary>
        /// Adds two polynomials
        /// </summary>
        /// <param name="a">The first polynomial</param>
        /// <param name="b">The second polynomial</param>
        /// <returns>New polynomial after adding</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if either of provided polynomials is null</exception>
        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            if (a is null)
                throw new PolynomialArgumentNullException("Polynomial a is null");

            if (b is null)
                throw new PolynomialArgumentNullException("Polynomial b is null");

            Polynomial result = new Polynomial(a.ToArray());

            foreach (var item in b.ToArray())
            {
                if (!result.ContainsMember(item.Degree) && item.Coefficient != 0)
                    result.AddMember(item);
                else if (result.ContainsMember(item.Degree))
                    result[result.Find(item.Degree).Degree] += item.Coefficient;
            }

            return result;
        }

        /// <summary>
        /// Subtracts two polynomials
        /// </summary>
        /// <param name="a">The first polynomial</param>
        /// <param name="b">The second polynomial</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if either of provided polynomials is null</exception>
        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            if (a is null)
                throw new PolynomialArgumentNullException("Polynomial a is null");

            if (b is null)
                throw new PolynomialArgumentNullException("Polynomial b is null");

            Polynomial result = new Polynomial(a.ToArray());

            foreach (var item in b.ToArray())
            {
                if (!result.ContainsMember(item.Degree) && item.Coefficient != 0)
                {
                    result.AddMember(item);
                    result[result.Find(item.Degree).Degree] *= -1;
                }
                else if (result.ContainsMember(item.Degree))
                    result[result.Find(item.Degree).Degree] -= item.Coefficient;
            }
            return result;
        }

        /// <summary>
        /// Multiplies two polynomials
        /// </summary>
        /// <param name="a">The first polynomial</param>
        /// <param name="b">The second polynomial</param>
        /// <returns>Returns new polynomial after multiplication</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if either of provided polynomials is null</exception>
        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            if (a is null)
                throw new PolynomialArgumentNullException("Polynomial a is null");

            if (b is null)
                throw new PolynomialArgumentNullException("Polynomial b is null");

            Polynomial result = new Polynomial();
            PolynomialMember tempPolynomial;

            foreach (var itemA in a.ToArray())
            {
                foreach (var itemB in b.ToArray())
                {
                    if (itemA.Coefficient * itemB.Coefficient != 0)
                    {
                        tempPolynomial = new PolynomialMember(itemA.Degree + itemB.Degree, itemA.Coefficient * itemB.Coefficient);

                        if (!result.ContainsMember(tempPolynomial.Degree))
                            result.AddMember(tempPolynomial);
                        else
                            result[result.Find(tempPolynomial.Degree).Degree] += tempPolynomial.Coefficient;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Adds polynomial to polynomial
        /// </summary>
        /// <param name="polynomial">The polynomial to add</param>
        /// <returns>Returns new polynomial after adding</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if provided polynomial is null</exception>
        public Polynomial Add(Polynomial polynomial)
        {
            if (polynomial is null)
                throw new PolynomialArgumentNullException("Polynomial is null");

            return this + polynomial;
        }

        /// <summary>
        /// Subtracts polynomial from polynomial
        /// </summary>
        /// <param name="polynomial">The polynomial to subtract</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if provided polynomial is null</exception>
        public Polynomial Subtraction(Polynomial polynomial)
        {
            if (polynomial is null)
                throw new PolynomialArgumentNullException("Polynomial is null");

            return this - polynomial;
        }

        /// <summary>
        /// Multiplies polynomial with polynomial
        /// </summary>
        /// <param name="polynomial">The polynomial for multiplication </param>
        /// <returns>Returns new polynomial after multiplication</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if provided polynomial is null</exception>
        public Polynomial Multiply(Polynomial polynomial)
        {
            if (polynomial is null)
                throw new PolynomialArgumentNullException("Polynomial is null");

            return this * polynomial;
        }

        
        /// <summary>
        /// Adds polynomial and tuple
        /// </summary>
        /// <param name="a">The polynomial</param>
        /// <param name="b">The tuple</param>
        /// <returns>Returns new polynomial after adding</returns>
        public static Polynomial operator +(Polynomial a, (double degree, double coefficient) b)
        {
            return a + new Polynomial((b.degree, b.coefficient));
        }

        /// <summary>
        /// Subtract polynomial and tuple
        /// </summary>
        /// <param name="a">The polynomial</param>
        /// <param name="b">The tuple</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        public static Polynomial operator -(Polynomial a, (double degree, double coefficient) b)
        {
            return a - new Polynomial((b.degree, b.coefficient));
        }

        /// <summary>
        /// Multiplies polynomial and tuple
        /// </summary>
        /// <param name="a">The polynomial</param>
        /// <param name="b">The tuple</param>
        /// <returns>Returns new polynomial after multiplication</returns>
        public static Polynomial operator *(Polynomial a, (double degree, double coefficient) b)
        {
            return a * new Polynomial((b.degree, b.coefficient));
        }

        /// <summary>
        /// Adds tuple to polynomial
        /// </summary>
        /// <param name="member">The tuple to add</param>
        /// <returns>Returns new polynomial after adding</returns>
        public Polynomial Add((double degree, double coefficient) member)
        {
            return this + (member.degree, member.coefficient) ;
        }

        /// <summary>
        /// Subtracts tuple from polynomial
        /// </summary>
        /// <param name="member">The tuple to subtract</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        public Polynomial Subtraction((double degree, double coefficient) member)
        {
            return this - (member.degree, member.coefficient);
        }

        /// <summary>
        /// Multiplies tuple with polynomial
        /// </summary>
        /// <param name="member">The tuple for multiplication </param>
        /// <returns>Returns new polynomial after multiplication</returns>
        public Polynomial Multiply((double degree, double coefficient) member)
        {
            return this * (member.degree, member.coefficient);
        }
    }
}
