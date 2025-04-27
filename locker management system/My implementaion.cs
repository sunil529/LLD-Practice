// ### Problem statement:
// Design a Locker management system for an ecommerce store. While performing a purchase, the users can opt for their packages to get delivered at a Locker system close to their place. The delivery person will place the package in a locker. An otp will be generated and sent to the user once the package is added to the locker.

// As per the user’s convenience, they can visit the locker, enter the otp and get their parcel. Further, if the users want to return the goods, they can visit the locker and place the item. Delivery guy will get an otp which can be used to unlock the locker.

// #### Following are the expectations from the system:
// 1.	Person asks the system to allocate a locker for a given package. Assume random allocation for now. The system should be extensible and accommodate allocation of locker based on the size of the input package
// 2.	The system must generate a code/otp and send it to the user along with the locker details
// 3.	The user can enter the otp & locker details to unlock the locker
// 4.	Once the package is taken out, the locker can be allocated for any other order
// 5.	Users can use the locker for returning an item. OTP/code will be sent to the delivery person in this case
// 6.	Operations/Admin can view & vacate all lockers which are in use for more than 3 days

// #### Evaluation criteria:
// 1.	Code readability
// 2.	Extensibility & re-usability
// 3.	Modularity
// 4.	Testability


// locker management system

// 1.Delivery Person->will placegoods to a locker(input size) (done)
// 2.Otp->User will recieve the details about the locker which has got assigned to his goods
// 3.User-> action 1->Enter OTP an retrieve the goods once it si out we need ot make it agian empty so that we can use it agian
//          actions 2->a)wants to return ->he needs to place the item agian in the locker
//          action 2 -> b)the delviery person will get an OTP


entity 

 public enum size 
 {
large 
medium
small
 }

public class Package
{
    int lockerId,
sizetype size,
int name,
User user
}

public class Locker{
    int id,
    Size size,
Package package,
string otp,
bool isEMpty,
}
public Interface IFindLockerStrategy
{
    FindLocker(package);
}
public class nearestLockerStrategy:IFindLockerStrategy
{
    public locker FindLocker(package)
    {
        
      for(int i=0;i<n;i++)
      {
         for(int j=0;j<n;j++)
         {
             if(package.size()<=locker[i][j].size&&locker.isEMpty)
             {
               return locker[i][j];
             }
         }
      }
      return null;
    }
}
public class LockerManager{
    int[][] locker;
    IFindLockerStrategy _strategy;
    public lockermanager(int n,IFindLockerStrategy _strategy)
    {
       locker=new int[n][n];
       int counter=1;
       for(int i=0;i<n;i++)
       {
        for(int j=0;j<n;j++)
        {
            if(counter<33)
            {
                currentsize=small;
            }
            else if(counter<66)
            {
                currentsize=medium;
            }
            else()
            {
                currentsize=large;
            }

            locker[i][j]=new locker{
                int id=counter,
                size=currentsize,
                Package package=null,
                string otp="",
                bool isEMpty=true
            }
            counter++;
        }
       }
    }
    public locker FindLocker(pakcage)
    {
         return _strategy.FindLocker(package);
    }
   bool  allocateLocker(lcoker,package);
   {
    if(lcoker!=null)
    {
        locker.package=package;
        package.lockerId = locker.id;
        locker.isEMpty=false;
        return true;
    }
    return false;
   }
   string generateOTP()
   {
     return string.random();
   }
   public locker findLockerbyId(string lockerID)
   {
    for(int i=0;i<n;i++)
    {
        for(int j=0;j<n;j++)
        {
            if(locker[i][j].lockerID=lockerId)
            return locker[i][j];
        }
    }
   }
}
public enum status{
    "In trasnit",
    "In Locker",
    "retrieved"
}

public class User:IUser{
    int name,
    int emailId,
    hashmap<package,string>packagevsotp=null;
    hashmap<package,status>packagevsstatsu;
    LockerManageMentSystem lockermanagementsystem;

    retrieve(package)
    {
       var result= lockermanagementsystem.retrrive(package,packagevsotp[package],packagevsstatus[package]);
       if(result)
       {
        packagevsotp[package]="";
        packagevsstatsu[package]= "retrieved"
       }
    }
}
public Interface IUser
{
    bool placegoodsLocker(package);
    returrngoods(package);
    
}
public class DeleveiryPerson:IUser
{
    int name,
    int emailId,
    Package package,
    private LockerManager lockermanager
    public DeleveiryPerson(package,lockermanager)
    {
        this.package=package;
        this.lockermanager=lockermanager;
    }

   public bool placegoodsLocker(package)
   {
    var locker=lockermanager.FindLocker(package);
    var result=lockermanager.alloacatelocker(locker,package);
    if(result)
    {
        string otp=lockermanager.generateOTP();
        package.user.packagevsotp[package]=otp;
        locker.otp=otp;
    }
   }
   

}

public class LockerManageMentSystem{
  private   LockerManager lockermanager;
      public LockerManageMentSystem(LockerManager lockermanager)
      {this.lockermanager=lockermanager;
      }

   public  bool retrievePackage(package package,string otp,string status)
   {
    if(status!="retrieved")
    {
        Locker locker =lockermanager.findLockerbyId(package.lockerId);
        if(locker!=null)
        {
         if(locker.otp==otp)
           locker.isempty=true;
           locker.package=null;
        }
    }
       
   }
}










