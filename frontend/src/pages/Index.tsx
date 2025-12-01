import Navbar from "@/components/Navbar";
import HeroSection from "@/components/HeroSection";
import FeaturedProducts from "@/components/FeaturedProducts";
import NewsletterPopup from "@/components/NewsletterPopup";
import Footer from "@/components/Footer";

const Index = () => {
  return (
    <div className="min-h-screen bg-background">
      <Navbar />
      <HeroSection />
      
      {/* Brand Story */}
      <section className="py-16 md:py-24 bg-card">
        <div className="container mx-auto px-4">
          <div className="max-w-3xl mx-auto text-center">
            <h2 className="text-3xl md:text-4xl font-display font-bold mb-6">
              A Legacy of Authentic Flavors
            </h2>
            <p className="text-lg text-muted-foreground mb-6">
              For over three generations, Sunfead has been the trusted name for authentic Indian 
              snacks and sweets. What started as a small shop in the bustling lanes of Old Delhi 
              has grown into a beloved brand, bringing traditional recipes and festive joy to 
              families across India.
            </p>
            <p className="text-lg text-muted-foreground">
              Every product is made with carefully sourced ingredients, time-honored techniques, 
              and the warmth of home-cooked goodness. From Diwali celebrations to everyday cravings, 
              Sunfead is here to make every moment special.
            </p>
          </div>
        </div>
      </section>

      <FeaturedProducts />
      
      {/* Why Choose Us */}
      <section className="py-16 md:py-24 bg-background">
        <div className="container mx-auto px-4">
          <div className="text-center mb-12">
            <h2 className="text-3xl md:text-4xl font-display font-bold mb-4">
              Why Choose Sunfead?
            </h2>
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="text-center p-6">
              <div className="text-5xl mb-4">🌾</div>
              <h3 className="text-xl font-semibold mb-2">Premium Ingredients</h3>
              <p className="text-muted-foreground">
                Only the finest quality ingredients sourced from trusted suppliers
              </p>
            </div>
            <div className="text-center p-6">
              <div className="text-5xl mb-4">👨‍🍳</div>
              <h3 className="text-xl font-semibold mb-2">Traditional Recipes</h3>
              <p className="text-muted-foreground">
                Authentic recipes passed down through generations
              </p>
            </div>
            <div className="text-center p-6">
              <div className="text-5xl mb-4">🚚</div>
              <h3 className="text-xl font-semibold mb-2">Fresh Delivery</h3>
              <p className="text-muted-foreground">
                Made fresh daily and delivered to your doorstep
              </p>
            </div>
          </div>
        </div>
      </section>

      <Footer />
      <NewsletterPopup />
    </div>
  );
};

export default Index;
